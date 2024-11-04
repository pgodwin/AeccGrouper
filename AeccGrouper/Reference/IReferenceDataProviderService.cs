
namespace AeccGrouper.Reference
{
    /// <summary>
    /// Interface to the reference data service
    /// </summary>
    public interface IReferenceDataProviderService : IDisposable
    {
      
        List<Ref_C1> GetRef_C1();
        List<Ref_D1> GetRef_D1();
        List<Ref_D2> GetRef_D2();
        List<Ref_D3> GetRef_D3();
        List<Ref_D4> GetRef_D4();
        List<Ref_E> GetRef_E();


        Ref_D2_TriageValue? GetTriageCategory(string ECDG, string episodeEndStatus, string transportMode, string triageCategory);

        Ref_D3_AgeGroup? GetAgeGroup(string ECDG, string ageBracket);
        Ref_C1? GetECDGCode(string principalDiaignosisShortCode);
        Ref_D1? GetIntercept(string ECDG, string ECDG_Subgroup);
        Ref_D4_Interaction? GetInteraction(string ECDG, string triageCategory, string ageBracket);
        List<Ref_E> GetFinalClass(string ECDG, double minScore);
    }
}