namespace Services.TextAnalysis.Models
{
    public class AnalysisResponse
    {
        public string Title { get; set; }
        public string Song { get; set; }
        public string Sentiment { get; set; }
        public string Url { get; set; }
    }
}