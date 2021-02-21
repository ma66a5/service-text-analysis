namespace Services.TextAnalysis.Services
{
    public interface ITokenService
    {
        string CreateToken(string secret);
    }
}