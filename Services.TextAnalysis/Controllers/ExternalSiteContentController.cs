using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Services.TextAnalysis.Models;
using Services.TextAnalysis.Services;

namespace Services.TextAnalysis.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExternalSiteContentController : ControllerBase
    {
        private readonly IAsyncPolicy<AnalysisResponse> _policy;
        private readonly IWebsiteService _websiteService;
        private readonly ISentimentAnalysisService _sentimentAnalysisService;
        private readonly ILogger<ExternalSiteContentController> _logger;

        public ExternalSiteContentController(
            IReadOnlyPolicyRegistry<string> policyRegistry,
            IWebsiteService websiteService,
            ISentimentAnalysisService sentimentAnalysisService,
            ILogger<ExternalSiteContentController> logger)
        {
            _policy = policyRegistry.Get<IAsyncPolicy<AnalysisResponse>>(Policies.AnalysisResponsePolicy);
            _websiteService = websiteService;
            _sentimentAnalysisService = sentimentAnalysisService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<AnalysisResponse> Get()
        {
            Func<Task<AnalysisResponse>> getAnalysisResponseFunc = async () =>
            {
                var content = await _websiteService.GetContentAsync();
                var sentiment = await _sentimentAnalysisService.DetectSentiment(content.Song, content.LanguageCode);
                return new AnalysisResponse
                {
                    Title = content.Title,
                    Sentiment = sentiment,
                    Song = content.Song,
                    Url = content.Url
                };
            };

            var context = new Context($"{nameof(ExternalSiteContentController)}.{nameof(ExternalSiteContentController.Get)}");

            return await _policy.ExecuteAsync((_) => getAnalysisResponseFunc(), context);
        }
    }
}
