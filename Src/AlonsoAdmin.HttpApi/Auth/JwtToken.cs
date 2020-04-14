using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Auth
{
    public class JwtToken
    {
        private readonly byte[] secret;
        private readonly string audience;
        private readonly string issuer;
        private readonly int expiresMinute;

        public JwtToken(JwtOption options)
        {
            secret = Encoding.ASCII.GetBytes(options.Secret);
            audience = options.Audience;
            issuer = options.Issuer;
            expiresMinute = options.ExpirationMinutes;
        }
        ///生成JWT
        public JwtResult GenerateToken(Claim[] claims)
        {
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddMinutes(expiresMinute);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return new JwtResult
            {
                access_token = tokenString,
                token_type = "Bearer",
                auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
            };
        }
    }
    public class JwtResult
    {
        /// <summary>
        /// access token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// token type
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 授权时间
        /// </summary>
        public long auth_time { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public long expires_at { get; set; }
    }
    public class JwtOption
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}
