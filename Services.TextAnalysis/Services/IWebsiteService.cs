using System.Threading.Tasks;
using Services.Models.Services;

namespace Services.TextAnalysis.Services
{
    public interface IWebsiteService
    {
        Task<SongResponse> GetContentAsync();
    }
}