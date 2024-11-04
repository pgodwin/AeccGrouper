using AeccGrouper.Reference;
using System.Text.RegularExpressions;

namespace AeccGrouper.Tests
{

    public class GrouperTests : IDisposable
    {
        private readonly ReferenceDataProviderService _referenceService;
        private readonly Grouper _grouper;

        public GrouperTests()
        {
            var connectionString = "Data Source=ecdg_values.db";
            this._referenceService = new ReferenceDataProviderService(connectionString);
            this._grouper = new Grouper(this._referenceService);
        }

        [Theory]
        [MemberData(nameof(GrouperTestData))]
        public void Grouper_Result_Match_AECC_Grouper(
            string episodeNumber,
            string triageCategory,
            string episodeEndStatus,
            string typeOfVisitToEd,
            string ageYears,
            string transportMode,
            string principalDiagnosisShortCode,
            string serviceDate,
            
            string subCategory,
            double complexityScore,
            string aeccEndClass
            )
        {
           var result = _grouper.Group(
               episodeNumber,
               triageCategory,
               episodeEndStatus,
               typeOfVisitToEd,
               ageYears,
               transportMode,
               principalDiagnosisShortCode,
               serviceDate
                );

            Assert.NotNull(result);

            Assert.Equal(episodeNumber, result.EpisodeNumber);
            Assert.Equal(triageCategory, result.TriageCategory);
            Assert.Equal(episodeEndStatus, result.EpisodeEndStatus);
            Assert.Equal(typeOfVisitToEd, result.TypeOfVisitToEd);
            Assert.Equal(ageYears, result.AgeYears);
            Assert.Equal(transportMode, result.TransportMode);
            Assert.Equal(principalDiagnosisShortCode, result.PrincipalDiagnosisShortCode);
            Assert.Equal(serviceDate, result.ServiceDate);

            // Our calculated valuess
            Assert.Equal(subCategory, result.ECDG_Subgroup);
            
            // There might be some floating point precision issues here, 14 places should be sufficient right?

            Assert.Equal(Math.Round(complexityScore, 14, MidpointRounding.AwayFromZero), Math.Round(result.ScaledComplexityScore, 14, MidpointRounding.AwayFromZero));

            Assert.Equal(aeccEndClass, result.AECC_EndClass);
        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// Grouper test data processed via the AECC Grouper Application
        /// </summary>
        public static IEnumerable<object[]> GrouperTestData => new List<object[]>
        {
            //  Input:
            //             EpisodeNumber,
            //             TriageCategory,
            //             EpisodeEndStatus,
            //             TypeOfVisitToEd,
            //             AgeYears,
            //             TransportMode,
            //             PrincipalDiagnosisShortCode,
            //             ServiceDate,
            
            //  Result:
            //             SubCategory
            //             ComplexityScore
            //             AECC_EndClass
            new object[] { "A2070005372002", "2", "1", "1", "18", "8", "F03",   "30/06/2022", "",      "0",                  "E9903Z" },
            new object[] { "A2070005372077", "4", "1", "1", "0",  "8", "T199",  "30/06/2022", "",      "0",                  "E9903Z" },
            new object[] { "A2070005372004", "4", "1", "1", "0",  "8", "K589",  "30/06/2022", "",      "0",                  "E9903Z" },
            new object[] { "A2070005372008", "4", "1", "1", "0",  "8", "R17",   "30/06/2022", "",      "0",                  "E9903Z" },
            new object[] { "A2070005371899", "1", "2", "1", "32", "1", "I495",  "30/06/2022", "",      "0",                  "E9903Z" },
            new object[] { "A2070005371552", "4", "1", "1", "66", "1", "F0300", "30/06/2022", "E0111", 4.493848272850942d,   "E0110B" },
            new object[] { "A2070005371896", "1", "2", "1", "32", "1", "F0301", "30/06/2022", "E0111", 3.410852160297093d,   "E0110B" },
            new object[] { "A2070005372003", "4", "1", "1", "0",  "8", "K588",  "30/06/2022", "E0651", 2.4056475232885672d,  "E0650C" },
            new object[] { "A2070005372007", "4", "1", "1", "0",  "8", "R170",  "30/06/2022", "E0711", 2.48288810860601d,    "E0710B" },
            new object[] { "A2070005372015", "4", "1", "1", "0",  "8", "S3151", "30/06/2022", "E2033", 1.4102278431530098d,  "E2030C" },
            new object[] { "A2070005372019", "4", "1", "1", "0",  "8", "S3152", "30/06/2022", "E2033", 1.4102278431530098d,  "E2030C" },
            new object[] { "A2070005372021", "2", "1", "1", "18", "8", "S3785", "1/07/2022",  "E2014", 7.991748192719114d,   "E2010B" },
            new object[] { "A2070005372024", "2", "1", "1", "18", "8", "S3786", "30/06/2022", "E2014", 7.991748192719114d,   "E2010B" },
            new object[] { "A2070005372033", "4", "1", "1", "0",  "8", "T193",  "30/06/2022", "E1391", 2.5161138720583027d,  "E1390B" },
            new object[] { "A2070005372049", "4", "1", "1", "0",  "8", "T1981", "30/06/2022", "E1291", 2.033697857612346d,   "E1290B" },
            new object[] { "A2070005372076", "4", "1", "1", "0",  "8", "T1982", "30/06/2022", "E1291", 2.033697857612346d,   "E1290B" },
            new object[] { "A2070005372009", "2", "1", "1", "18", "8", "I4951", "30/06/2022", "E0521", 4.822256720769615d,   "E0520A" },


        };
    }
}