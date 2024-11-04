// C# implementation of AeccGrouper for integration with other applications.

// stateid,edtriag,eddepst,edvisit,ageyears,transmode,x11ddx1,servdate
// Output fields:
// ECDG_SubGroup
// ComplexityScore
// AECC_EndClass

using CsvHelper.Configuration.Attributes;

public class AeccFile
{
    public string stateid { get; set; }
    public string edtriag { get; set; }
    public string eddepst { get; set; }
    public string edvisit { get; set; }
    public string ageyears { get; set; }
    public string transmode { get; set; }
    public string x11ddx1 { get; set; }
    public string servdate { get; set; }

    [Optional]

    public string ECDG_Subgroup { get; set; }

    [Optional]

    public double ComplexityScore { get; set; }

    [Optional]

    public string AECC_EndClass { get; set; }
}