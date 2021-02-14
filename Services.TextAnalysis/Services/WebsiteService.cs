using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Services.Models.Services;

namespace Services.TextAnalysis.Services
{
    public class WebsiteService : IWebsiteService
    {
        private const string LiederUrl = "?TextId={0}";

        private readonly Random _random = new Random();
        private readonly HttpClient _client;

        public WebsiteService(HttpClient client)
        {
            _client = client;
        }

        public async Task<SongResponse> GetContentAsync()
        {
            int randomInt = _random.Next(99999);
            using var response = await _client.GetAsync(string.Format(LiederUrl, randomInt));
            using var content = response.Content;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(await content.ReadAsStringAsync());

            var title = htmlDoc.DocumentNode.SelectSingleNode(@"//title").InnerHtml;
            var song = htmlDoc.DocumentNode.SelectSingleNode(@"//div[@id=""the-tr""]/pre").InnerHtml;
            var language = htmlDoc.DocumentNode.SelectSingleNode(@"//div[@id=""tr-lang""]/span").InnerHtml;

            return new SongResponse
            {
                Title = title,
                Song = song,
                LanguageCode = GetLanguageCode(language)
            };
        }

        private static string GetLanguageCode(string language) =>
            language switch
            {
                string l when l.IndexOf("German", StringComparison.OrdinalIgnoreCase) >= 0 => "de",
                string l when l.IndexOf("French", StringComparison.OrdinalIgnoreCase) >= 0 => "fr",
                string l when l.IndexOf("Italian", StringComparison.OrdinalIgnoreCase) >= 0 => "it",
                string l when l.IndexOf("Spanish", StringComparison.OrdinalIgnoreCase) >= 0 => "es",
                string l when l.IndexOf("Portuguese", StringComparison.OrdinalIgnoreCase) >= 0 => "pt",
                _ => "en"
            };
    }
}