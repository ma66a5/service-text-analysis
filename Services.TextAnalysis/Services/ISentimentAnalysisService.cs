using System.Threading.Tasks;

namespace Services.TextAnalysis.Services
{
    public interface ISentimentAnalysisService
    {
        Task<string> DetectSentiment(string text, string languageCode);
    }
}