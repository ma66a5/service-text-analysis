using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.TextAnalysis.Configuration;
using Service.TextAnalysis.Contracts;
using Service.TextAnalysis.Models;

namespace Service.TextAnalysis.Controllers
{
    // TODO: This should be housed in a separate service.
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
            {
                return BadRequest(new { message = "User name or password is incorrect" });
            }

            return Ok(user);
        }
    }
}
