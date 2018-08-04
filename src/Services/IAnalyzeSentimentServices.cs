namespace Service.TextAnalysis.Services
{
    public interface IAnalyzeSentimentService
    {
        string GetSentiment(string text);
    }
}