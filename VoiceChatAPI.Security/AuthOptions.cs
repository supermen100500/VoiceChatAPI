using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VoiceChatAPI.Domain.Models;
using VoiceChatAPI.Security.Models;

namespace VoiceChatAPI.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "VoiceChatServer"; // издатель токена
        public const string AUDIENCE = "VoiceChatClient"; // потребитель токена
        const string KEY = "ZDfmUCWjIJNPqLZr";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 30 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public JWTPair GenerateJWTToken(AppUser user, string role = "User")
        {
            var identity = GetIdentity(user, role);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new JWTPair
            {
                AccessToken = encodedJwt,
                UserName = identity.Name
            };

            return response;
        }

        private ClaimsIdentity GetIdentity(AppUser user, string role = "User")
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                return DateTime.UtcNow < expires;
            }
            return false;
        }
    }
}
