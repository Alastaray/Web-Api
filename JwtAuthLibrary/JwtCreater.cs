using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtCreaterLibrary
{
    public class JwtCreater
    {
        public static string Create(ClaimsIdentity identity)
        {
            var jwt = new JwtSecurityToken(
                    claims: identity.Claims,
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
        private static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            string? key = ConfigurationManager.AppSettings["JwtKey"];
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key ?? "qwertyuiopasdfghjklzxcvbnm"));
        }
    }
}