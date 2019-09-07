using System;
using System.Threading.Tasks;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Service.TextAnalysis.Contracts;

namespace Service.TextAnalysis.Services
{
    public class AnalyzeSentimentService : IAnalyzeSentimentService
    {
        public AnalyzeSentimentService(IAmazonComprehend comprehendClient)
        {
            if (comprehendClient == null) throw new ArgumentNullException(nameof(comprehendClient));

            _comprehendClient = comprehendClient;
        }

        private IAmazonComprehend _comprehendClient;

        public async Task<string> GetSentiment(string text)
        {
            var detectSentimentRequest = new DetectSentimentRequest
            {
                Text = text,
                LanguageCode = "en"
            };
            var detectSentimentResponse = await _comprehendClient.DetectSentimentAsync(detectSentimentRequest);
            return detectSentimentResponse.Sentiment;
        }
    }
}