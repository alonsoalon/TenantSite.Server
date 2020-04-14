
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.MultiTenant.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlonsoAdmin.Common.Auth
{
    public class AuthToken: IAuthToken
    {
        private JwtConfig JwtConfig
        {
            get
            {
                var ti = _accessor.HttpContext.GetMultiTenantContext()?.TenantInfo;
                var option = new JwtConfig();
                if (ti.Items.TryGetValue("Audience", out object audience))
                {
                    option.Audience = audience.ToString();
                }
                if (ti.Items.TryGetValue("Secret", out object secret))
                {
                    option.Secret = (string)secret;
                }
                if (ti.Items.TryGetValue("Issuer", out object issuer))
                {
                    option.Issuer = (string)issuer;
                }
                if (ti.Items.TryGetValue("ExpirationMinutes", out object expirationMinutes))
                {
                    if (int.TryParse(expirationMinutes.ToString(), out int expi))
                    {
                        option.ExpirationMinutes = expi;
                    }
                    else
                    {
                        option.ExpirationMinutes = 30;
                    }
                }
                else
                {
                    option.ExpirationMinutes = 30;
                }
                return option;
            }
        }
        private IHttpContextAccessor _accessor;
        public AuthToken(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Build(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtConfig.Issuer,
                audience: JwtConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(JwtConfig.ExpirationMinutes),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
