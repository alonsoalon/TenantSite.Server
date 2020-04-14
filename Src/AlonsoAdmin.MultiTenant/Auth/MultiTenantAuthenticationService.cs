using AlonsoAdmin.MultiTenant.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Auth
{
    internal class MultiTenantAuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationService inner;

        public MultiTenantAuthenticationService(IAuthenticationService inner)
        {
            this.inner = inner ?? throw new System.ArgumentNullException(nameof(inner));
        }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
            => inner.AuthenticateAsync(context, scheme);

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            // Add tenant identifier to the properties so on the callback we can use it to set the multitenant context.
            var multiTenantContext = context.GetMultiTenantContext();
            if (multiTenantContext.TenantInfo != null)
            {
                properties = properties ?? new AuthenticationProperties();
                properties.Items.Add("tenantId", multiTenantContext.TenantInfo.Id);
            }

            return inner.ChallengeAsync(context, scheme, properties);
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
            => inner.ForbidAsync(context, scheme, properties);

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
            => inner.SignInAsync(context, scheme, principal, properties);

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
            => inner.SignOutAsync(context, scheme, properties);
    }
}
