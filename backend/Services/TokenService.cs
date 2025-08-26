using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            // The key is created from the SigningKey in appsettings.json
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _userManager = userManager;
        }

        public Task<string> CreateToken(AppUser user)
        {
            // These are the "claims" or pieces of information we want to include in the token
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            // This creates a security credentials using the key and a hashing algorithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            // This descriptor contains all the information about the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
