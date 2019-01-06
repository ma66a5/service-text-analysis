using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.TextAnalysis.Configuration;

namespace Service.TextAnalysis.Controllers
{
    // TODO: This should be housed in a separate service.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController
    {
        private readonly AuthenticationSettings _appSettings;

        public AuthenticationController(IOptions<AuthenticationSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<string> GetAsync()
        {
            // TODO: Move this logic out to a service.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "API Consumer")
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
