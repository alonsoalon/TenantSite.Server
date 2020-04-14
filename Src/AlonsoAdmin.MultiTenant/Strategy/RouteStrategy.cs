using AlonsoAdmin.MultiTenant.Error;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class RouteStrategy : IMultiTenantStrategy
    {

        internal readonly string tenantParam;

        public RouteStrategy(string tenantParam)
        {
            if (string.IsNullOrWhiteSpace(tenantParam))
            {
                throw new ArgumentException($"\"{nameof(tenantParam)}\" 不能为空 ", nameof(tenantParam));
            }

            this.tenantParam = tenantParam;
        }

        public async Task<string> GetIdentifierAsync(object context)
        {

            if (!(context is HttpContext))
                throw new MultiTenantException(null,
                    new ArgumentException($"\"{nameof(context)}\" 必须是HttpContext类型 ", nameof(context)));

            var httpContext = context as HttpContext;

            object identifier = null;
            httpContext.Request.RouteValues.TryGetValue(tenantParam, out identifier);

            return await Task.FromResult(identifier as string);
        }
    }
}
