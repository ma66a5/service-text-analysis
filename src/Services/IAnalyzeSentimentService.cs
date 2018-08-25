using System.Threading.Tasks;

namespace Service.TextAnalysis.Services
{
    public interface IAnalyzeSentimentService
    {
        Task<string> GetSentiment(string text);
    }
}