using System.Threading.Tasks;
using Amazon.Comprehend;

namespace Services.TextAnalysis.Services
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly IAmazonComprehend _comprehendService;

        public SentimentAnalysisService(IAmazonComprehend comprehendService)
        {
            _comprehendService = comprehendService;
        }

        public async Task<string> DetectSentiment(string text, string languageCode)
        {
            Amazon.Comprehend.Model.DetectSentimentRequest request = new Amazon.Comprehend.Model.DetectSentimentRequest
            {
                Text = text,
                LanguageCode = languageCode
            };

            var analysis = await _comprehendService.DetectSentimentAsync(request);

            return analysis.Sentiment;
        }
    }
}