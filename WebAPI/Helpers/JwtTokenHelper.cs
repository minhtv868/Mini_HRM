using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Helpers
{
    public static class JwtTokenHelper
    {
        private const double EXPIRY_DURATION_MINUTES = 3;
        public static string BuildToken(string key, string issuer)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name,issuer),
                new Claim(ClaimTypes.Role, issuer)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public static bool IsTokenValid(string key, string issuer, string token)
        {

            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string ReadToken(string key, string issuer, string token)
        {

            string tokenDecript = "";
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var info = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
                var sessionRead = info.Claims.FirstOrDefault(c => c.Type == "Session").Value;

                tokenDecript += "<br>Issuer: " + validatedToken.Issuer;
                tokenDecript += "<br>ValidFrom: " + validatedToken.ValidFrom;
                tokenDecript += "<br>ValidTo: " + validatedToken.ValidTo;
                tokenDecript += "<br>Session: " + sessionRead;
            }
            catch
            {
                return tokenDecript;
            }
            return tokenDecript;
        }
    }
}
