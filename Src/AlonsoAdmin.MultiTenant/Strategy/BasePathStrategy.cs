using AlonsoAdmin.MultiTenant.Error;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class BasePathStrategy : IMultiTenantStrategy
    {
        public async Task<string> GetIdentifierAsync(object context)
        {
            if (!(context is HttpContext))
                throw new MultiTenantException(null,
                    new ArgumentException($"\"{nameof(context)}\" 必须是HttpContext类型 ", nameof(context)));

            var path = (context as HttpContext).Request.Path;

            var pathSegments =
                path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (pathSegments.Length == 0)
                return null;

            string identifier = pathSegments[0];

            return await Task.FromResult(identifier); 
        }
    }
}
