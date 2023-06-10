using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Services.Contracts;
using static Common.Constants;

namespace UserManagement.Services
{
    public class JwtHelper : IJwtHelper
    {
        public string GetNewJwtToken(List<Claim> userClaims, string userId)
        { 
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY_VALUE));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:7264",
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCredentials);
            string stringToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return stringToken;
        }
    }
}
