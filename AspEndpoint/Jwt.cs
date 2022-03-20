using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspEndpoint
{
    public static class Jwt
    {
        public static string Create(ClaimsIdentity identity)
        {
            var jwt = new JwtSecurityToken(
                    claims: identity.Claims,  
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddSeconds(GetLifeTime()),
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
        public static bool ValidateLifeTime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var now = DateTime.UtcNow;
            return !(notBefore > now || now > expires);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            string? key = System.Configuration.ConfigurationManager.AppSettings["JwtKey"];
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key ?? "qwertyuiopasdfghjklzxcvbnm"));
        }
        public static int GetLifeTime()
        {
            return int.Parse(System.Configuration.ConfigurationManager.AppSettings["JwtLifeTime"] ?? "60");
        }
    }
}