using Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.Authentication
{
    public class UserService
    {
        private readonly UserRoleService userRoleService;
        private readonly IConfiguration configuration;

        public UserService(UserRoleService userRoleService, IConfiguration configuration)
        {
            this.userRoleService = userRoleService;
            this.configuration = configuration;
        }

        public async Task<(JwtSecurityToken token, string tokenString)> GenerateToken(SystemUser user)
        {
            var authClaims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, user.Id) };
            var token = GetToken(authClaims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (token, tokenString);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            string secret = configuration["JWT:Secret"] ?? string.Empty;
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
