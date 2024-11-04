using Dapper;
using System.Data.SQLite;

namespace AeccGrouper.Reference
{
    /// <summary>
    /// SQLite implementation of grouper reference data
    /// </summary>
    /// <param name="connectionString"></param>
    public class ReferenceDataProviderService(string connectionString) : IReferenceDataProviderService
    {
        private readonly string connectionString = connectionString;

        public Ref_D3_AgeGroup? GetAgeGroup(string ECDG, string ageBracket)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Ref_D3_AgeGroup>(
                "SELECT " +
                "ECDG_code, " +
                "@ageBracket as AgeBracket, " +
                "CASE @ageBracket " +
                    "WHEN '0-4' THEN 0 " + // todo double-check this mapping
                    "WHEN '5-9' THEN agegroup59 " +
                    "WHEN '10-14' THEN agegroup1014 " +
                    "WHEN '15-69' THEN agegroup1569 " +
                    "WHEN '70-74' THEN agegroup7074 " +
                    "WHEN '75-79' THEN agegroup7579 " +
                    "WHEN '80-84' THEN agegroup8084 " +
                    "WHEN '85+' THEN agegroup85 " +
                "END AS AgeGroupValue " +
                "FROM " +
                "Ref_ECDG_D3_Age_groups " +
                "WHERE ECDG_code=@ECDG",
                new { ECDG, ageBracket });
            }
        }

        public Ref_C1? GetECDGCode(string principalDiaignosisShortCode)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Ref_C1>(
                    "SELECT * FROM " +
                    "Ref_ECDG_C1_Short_list_code_maps " +
                    "WHERE Shortlist_code=@principalDiaignosisShortCode", 
                    new { principalDiaignosisShortCode });
            }
        }

        public Ref_D2_TriageValue? GetTriageCategory(string ECDG, string episodeEndStatus, string transportMode, string triageCategory)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Ref_D2_TriageValue>(
                    "SELECT " +
                    "ECDG_Code, " +
                    "@episodeEndStatus as EpisodeEndStatus, " +
                    "CASE @episodeEndStatus " +
                        "WHEN '1' THEN Admitted " +
                        "WHEN '3' THEN Referred " +
                        "WHEN '5' THEN Left_At_Own_Risk " +
                        "WHEN '6' THEN Died_In_Ed " +
                    "END AS EpisodeEndStatusValue, " +
                    "@transportMode as TransportMode, " +
                    "CASE @transportMode " +
                        "WHEN '1' THEN Arrival_By_Ambulance " +
                    "END AS TransportModeValue, " +
                    "@triageCategory as TriageCategory," +
                    "CASE @triageCategory " +
                        "WHEN '1' THEN Triage_category1 " +
                        "WHEN '2' THEN Triage_category2 " +
                        "WHEN '3' THEN Triage_category3 " +
                        "WHEN '4' THEN Triage_category4 " +
                    "END AS TriageCategoryValue " +
                    "FROM " +
                    "Ref_ECDG_D2_TriageCategories " +
                    "WHERE ECDG_code=@ECDG",
                    new { ECDG, episodeEndStatus, transportMode, triageCategory });
            }
        }

        public Ref_D4_Interaction? GetInteraction(string ECDG, string triageCategory, string ageBracket)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Ref_D4_Interaction>(
                        "SELECT " +
                        "ECDG_code, " +
                        "@ageBracket as AgeBracket, " +
                            "CASE @ageBracket " +
                            "WHEN '0-4' THEN agegroup04 " +
                            "WHEN '5-9' THEN agegroup59 " +
                            "WHEN '10-14' THEN agegroup1014 " +
                            "WHEN '80-84' THEN agegroup8084 " +
                            "WHEN '85+' THEN agegroup85 " +
                        "END AS AgeValue, " +
                        "@triageCategory as TriageCategory, " +
                        "CASE @triageCategory " +
                            "WHEN '1' THEN Triage_category1 " +
                            "WHEN '2' THEN Triage_category2 " +
                            "WHEN '3' THEN Triage_category3 " +
                            "WHEN '4' THEN Triage_category4 " +
                        "END AS TriageValue " +
                        "FROM " +
                        "Ref_ECDG_D4_Interactions " +
                        "WHERE ECDG_code=@ECDG",
                        new { ECDG, triageCategory, ageBracket });
            }
        }

        public Ref_D1? GetIntercept(string ECDG, string ECDG_Subgroup)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Ref_D1>(
                    "SELECT * FROM " +
                    "Ref_ECDG_D1_Intercept " +
                    "WHERE ECDG_code=@ECDG AND ECDG_Sub=@ECDG_Subgroup",
                    new { ECDG, ECDG_Subgroup });
            }
        }

        public List<Ref_C1> GetRef_C1()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ref_C1>("SELECT * FROM Ref_ECDG_C1_Short_list_code_maps").ToList();
            }
        }

        public List<Ref_D1> GetRef_D1()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ref_D1>("SELECT * FROM Ref_ECDG_D1_Intercept").ToList();
            }
        }

        public List<Ref_D2> GetRef_D2()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ref_D2>("SELECT * FROM Ref_ECDG_D2_TriageCategories").ToList();
            }
        }

        public List<Ref_D3> GetRef_D3()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ref_D3>("SELECT * FROM Ref_ECDG_D3_Age_groups").ToList();
            }
        }

        public List<Ref_D4> GetRef_D4()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ref_D4>("SELECT * FROM Ref_ECDG_D4_Interactions").ToList();
            }
        }

        public List<Ref_E> GetRef_E()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Ref_E>("SELECT * FROM Ref_ECDG_E_Complexity_score_thresholds").ToList();
            }
        }

      

        public List<Ref_E> GetFinalClass(string ECDG, double minScore)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                return connection
                    .Query<Ref_E>(
                        "SELECT * " +
                        "FROM Ref_ECDG_E_Complexity_score_thresholds " +
                        "WHERE ECDG_code = @ECDG " +
                        "AND @minScore >= [Min]" +
                        "ORDER BY [Min] DESC",
                        new { ECDG, minScore })
                    .ToList();
            }
        }
    }


}
