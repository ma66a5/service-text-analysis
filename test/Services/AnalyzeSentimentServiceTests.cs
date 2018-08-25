using System;
using System.Threading.Tasks;
using Amazon.Comprehend;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.TextAnalysis.Services;

namespace Service.TextAnalysis.Test.Services
{
    [TestClass]
    public class AnalyzeSentimentServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AnalyzeSentimentServiceThrowsExceptionIfAmazonComprehendIsNull()
        {
            var service = new AnalyzeSentimentService(null);
        }

        [TestMethod]
        public async Task GetSentimentExecutesAsExpected()
        {
            var options = new AWSOptions();
            options.Profile = "Development";
            options.Region = Amazon.RegionEndpoint.USEast2;
            var comprehendClient = options.CreateServiceClient<IAmazonComprehend>();
            var service = new AnalyzeSentimentService(comprehendClient);
            var text = "I love you.";
            var sentiment = await service.GetSentiment(text);
            Assert.AreEqual("POSITIVE", sentiment);
        }
    }
}
