using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccGrouper.Reference
{

    /*
     CREATE TABLE "Ref_ECDG_C1_Short_list_code_maps" (
	"ECC_code"	TEXT,
	"ECC_code_label"	TEXT,
	"ECDG_code"	TEXT,
	"ECDG_label"	TEXT,
	"ECDG_sub"	TEXT,
	"ECDG_sub_label"	TEXT,
	"Shortlist_code"	TEXT,
	"Term"	TEXT
    );
    */
    public class Ref_C1
    {
        public string ECC_code { get; set; }
        public string ECC_code_label { get; set; }
        public string ECDG_code { get; set; }
        public string ECDG_label { get; set; }

        public string ECDG_sub { get; set; }

        public string ECDG_sub_label { get; set; }

        public string Shortlist_code { get; set; }

        public string Term { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Ref_D1
    {
        public string ECDG_code { get; set; }
        public string ECDG_sub { get; set; }

        public int Intercept { get; set; }

        public double Intercept_sub { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /*
     * CREATE TABLE "Ref_ECDG_D2_TriageCategories" (
        "ECDG_code"	TEXT,
        "Admitted"	REAL,
        "Died_in_ED"	REAL,
        "Left_at_own_risk"	REAL,
        "Referred"	REAL,
        "Arrival_by_ambulance"	REAL,
        "Triage_category1"	REAL,
        "Triage_category2"	REAL,
        "Triage_category3"	REAL,
        "Triage_category4"	REAL
    );
    */
    public class Ref_D2
    {
        public string ECDG_code { get; set; }
        public double Admitted { get; set; }
        public double Died_in_ED { get; set; }

        public double Left_at_own_risk { get; set; }
        public double Referred { get; set; }
        public double Arrival_by_ambulance { get; set; }

        public double Triage_category1 { get; set; }
        public double Triage_category2 { get; set; }
        public double Triage_category3 { get; set; }
        public double Triage_category4 { get; set; }
    }

    public class Ref_D2_TriageValue
    {
        public string ECDG_code { get; set; }

        public string EpisodeEndStatus { get; set; }

        public double EpisodeEndStatusValue { get; set; }

        public string TransportMode { get; set; }

        public double TransportModeValue { get; set; }


        public string TriageCategory { get; set; }
        public double TriageCategoryValue { get; set; }
    }

    /*
     * CREATE TABLE "Ref_ECDG_D3_Age_groups" (
        "ECDG_code"	TEXT,
        "agegroup59"	REAL,
        "agegroup1014"	REAL,
        "agegroup1569"	REAL,
        "agegroup7074"	REAL,
        "agegroup7579"	REAL,
        "agegroup8084"	REAL,
        "agegroup85"	REAL
    );
    */
    public class Ref_D3
    {
        public string ECDG_code { get; set; }
        public double Agegroup59 { get; set; }
        public double Agegroup1014 { get; set; }
        public double Agegroup1569 { get; set; }
        public double Agegroup7074 { get; set; }
        public double Agegroup7579 { get; set; }
        public double Agegroup8084 { get; set; }
        public double Agegroup85 { get; set; }
    }

    public class Ref_D3_AgeGroup
    {
        public string ECDG_code { get; set; }
        public string AgeBracket { get; set; }
        public double AgeGroupValue { get; set; }
    }

    public class Ref_D4_Interaction
    {
        public string ECDG_code { get; set; }
        public string AgeBracket { get; set; }
        public double AgeValue { get; set; }

        public string TriageCategory { get; set; }
        public double TriageValue { get; set; }
        
    }

    /*
     CREATE TABLE "Ref_ECDG_D4_Interactions" (
        "ECDG_code"	TEXT,
        "agegroup04"	REAL,
        "agegroup59"	REAL,
        "agegroup1014"	REAL,
        "agegroup8084"	REAL,
        "agegroup85"	REAL,
        "Triage_category1"	REAL,
        "Triage_category2"	REAL,
        "Triage_category3"	REAL,
        "Triage_category4"	REAL
    );
    */
    public class Ref_D4
    {
        public string ECDG_code { get; set; }
        public double AgeGroup04 { get; set; }
        public double AgeGroup59 { get; set; }

        public double Agegroup1014 { get; set; }

        public double Agegroup8084 { get; set; }
        public double Agegroup85 { get; set; }

        public double Triage_category1 { get; set; }

        public double Triage_category2 { get; set; }

        public double Triage_category3 { get; set; }

        public double Triage_category4 { get; set; }

    }

    /*
     CREATE TABLE "Ref_ECDG_E_Complexity_score_thresholds" (
	"ECDG_code"	TEXT,
	"Label"	TEXT,
	"AECC_class"	TEXT,
	"Min"	REAL,
	"Max"	TEXT
);*/

    public class Ref_E
    {
        public string ECDG_code { get; set; }
        public string Label { get; set; }
        public string AECC_class { get; set; }
        public double Min { get; set; }
        public string Max { get; set; } // todo - this has "Infinity" in the database
    }


}
