using System;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;

namespace Service.TextAnalysis.Services
{
    public class AnalyzeSentimentService : IAnalyzeSentimentService
    {
        public string GetSentiment(string text)
        {
            var comprehendClient = new AmazonComprehendClient(Amazon.RegionEndpoint.USEast2);
            throw new NotImplementedException();
        }
    }
}