using Service.TextAnalysis.Models;

namespace Service.TextAnalysis.Contracts
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}