using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.TextAnalysis.Services;

namespace Services.TextAnalysis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public UserController(
            ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpGet]
        public string GetJwt(string secret)
            => _tokenService.CreateToken(secret);
    }
}