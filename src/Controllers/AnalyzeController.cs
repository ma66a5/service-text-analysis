using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.TextAnalysis.Contracts;
using Service.TextAnalysis.Services;

namespace Service.TextAnalysis.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        private readonly IAnalyzeSentimentService _analyzeSentimentService;

        public AnalyzeController(IAnalyzeSentimentService analyzeSentimentService)
        {
            _analyzeSentimentService = analyzeSentimentService ?? throw new ArgumentNullException(nameof(analyzeSentimentService));
        }

        [HttpGet("{text}")]
        public async Task<string> GetAsync(string text)
        {
            return await _analyzeSentimentService.GetSentiment(text);
        }
    }
}
