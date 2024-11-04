namespace AeccGrouper
{
    public class GrouperResult
    {

        public string EpisodeNumber { get; set; } = string.Empty;
        public string TriageCategory { get; set; } = string.Empty;

        public string EpisodeEndStatus { get; set; } = string.Empty;

        public string TypeOfVisitToEd { get; set; } = string.Empty;

        public string AgeYears { get; set; } = string.Empty;

        public string TransportMode { get; set; } = string.Empty;

        public string PrincipalDiagnosisShortCode { get; set; } = string.Empty;

        public string ServiceDate { get; set; } = string.Empty;

        public double InterceptScore { get; set; } = 0;

        public string AECC_EndClass { get; set; } = string.Empty;
        public double SubInterceptScore { get; set; } = 0;

        public double TransportModeScore { get; set; } = 0;
        public double EpisodeEndStatusScore { get; set; } = 0;

        public double TriageCategoryScore { get; set; } = 0;

        public double AgeGroupScore { get; set; } = 0;

        public double TriageInteractionScore { get; set; } = 0;

        public double AgeInteractionScore { get; set; } = 0;

        public double PredicatedValue { get; set; } = 0;
        public double ScaledComplexityScore { get; set; } = 0;
        public string ECDG { get; set; } = string.Empty;
        public string ECDG_Subgroup { get; set; } = string.Empty;
        public double InteractionScore { get; set; } = 0;
        public string AgeBracket { get; internal set; }
    }
}
