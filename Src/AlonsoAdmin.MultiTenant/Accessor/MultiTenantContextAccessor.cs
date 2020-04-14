using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using AlonsoAdmin.MultiTenant.Extensions;

namespace AlonsoAdmin.MultiTenant.Accessor
{
    public class MultiTenantContextAccessor : IMultiTenantContextAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MultiTenantContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public MultiTenantContext MultiTenantContext => httpContextAccessor.HttpContext?.GetMultiTenantContext();
    }
}
