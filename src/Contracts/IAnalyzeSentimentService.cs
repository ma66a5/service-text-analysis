using System.Threading.Tasks;

namespace Service.TextAnalysis.Contracts
{
    public interface IAnalyzeSentimentService
    {
        Task<string> GetSentiment(string text);
    }
}