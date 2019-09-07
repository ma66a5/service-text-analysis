using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.TextAnalysis.Configuration;
using Service.TextAnalysis.Contracts;
using Service.TextAnalysis.Models;

namespace Service.TextAnalysis.Services
{
    internal sealed class UserService : IUserService
    {
        private static readonly IEnumerable<User> _users = new List<User>()
        {
            new User
            {
                Id = Guid.Parse("f963806a-5d19-47ac-b143-2495a50faec1"),
                Username = "internal-process",
                Password = "Password@123"
            }
        };

        private readonly AuthenticationSettings _authSettings;

        public UserService(IOptions<AuthenticationSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }
    }
}