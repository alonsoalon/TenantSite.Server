using AlonsoAdmin.MultiTenant.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
            => builder.UseMiddleware<MultiTenantMiddleware>();
    }
}
