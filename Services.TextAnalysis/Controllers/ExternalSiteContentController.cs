using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.TextAnalysis.Models;
using Services.TextAnalysis.Services;

namespace Services.TextAnalysis.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ExternalSiteContentController : ControllerBase
    {
        private readonly IWebsiteService _websiteService;
        private readonly ISentimentAnalysisService _sentimentAnalysisService;
        private readonly ILogger<ExternalSiteContentController> _logger;

        public ExternalSiteContentController(
            IWebsiteService websiteService,
            ISentimentAnalysisService sentimentAnalysisService,
            ILogger<ExternalSiteContentController> logger)
        {
            _websiteService = websiteService;
            _sentimentAnalysisService = sentimentAnalysisService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<AnalysisResponse> Get()
        {
            var content = await _websiteService.GetContentAsync();
            var sentiment = await _sentimentAnalysisService.DetectSentiment(content.Song, content.LanguageCode);
            return new AnalysisResponse
            {
                Title = content.Title,
                Sentiment = sentiment,
                Song = content.Song
            };
        }
    }
}
