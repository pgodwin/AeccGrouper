namespace AeccGrouper
{
    public class GrouperResult
    {

        public string EpisodeNumber { get; set; }
        public string TriageCategory { get; set; }

        public string EpisodeEndStatus { get; set; }
        
        public string TypeOfVisitToEd { get; set; }

        public string AgeYears { get; set; }

        public string TransportMode { get; set; }

        public string PrincipalDiagnosisShortCode { get; set; }

        public string ServiceDate { get; set; }

        public double InterceptScore { get; set; } = 0;

        public string AECC_EndClass { get; set; } = string.Empty;
        public double SubInterceptScore { get; set; } = 0;

        public double TransportModeScore { get; set; } = 0;
        public double EpisodeEndStatusScore { get; set; } = 0;

        public double TriageCategoryScore { get; set; } = 0;

        public double AgeGroupScore { get; set; } = 0;

        public double TriageInteractionScore { get; set; } = 0;

        public double AgeInteractionScore { get; set; } = 0;

        public double PredicatedValue { get; set; }
        public double ScaledComplexityScore { get; set; }
        public string ECDG { get; set; }
        public string ECDG_Subgroup { get; set; }
        public double InteractionScore { get; set; }
    }
}
