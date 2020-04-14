using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Store
{
    public class HttpRemoteStore: IMultiTenantStore
    {
        internal const string defaultEndpointTemplateIdentifierToken = "{__tenant__}";
        private readonly HttpRemoteStoreClient client;
        private readonly string endpointTemplate;

        public HttpRemoteStore(HttpRemoteStoreClient client, string endpointTemplate)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            if (!endpointTemplate.Contains(defaultEndpointTemplateIdentifierToken))
            {
                if (endpointTemplate.EndsWith("/"))
                    endpointTemplate += defaultEndpointTemplateIdentifierToken;
                else
                    endpointTemplate += $"/{defaultEndpointTemplateIdentifierToken}";
            }

            if (Uri.IsWellFormedUriString(endpointTemplate, UriKind.Absolute))
                throw new ArgumentException("参数'endpointTemplate'不是uri格式.", nameof(endpointTemplate));

            if (!endpointTemplate.StartsWith("https", StringComparison.OrdinalIgnoreCase)
                && !endpointTemplate.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("参数'endpointTemplate'需以http或https开头.", nameof(endpointTemplate));

            this.endpointTemplate = endpointTemplate;
        }

        public Task<bool> TryAddAsync(TenantInfo tenantInfo)
        {
            throw new System.NotImplementedException();
        }

        public Task<TenantInfo> TryGetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TenantInfo> TryGetByCodeAsync(string code)
        {
            var result = await client.TryGetByCodeAsync(endpointTemplate, code);
            return result;
        }

        public Task<bool> TryRemoveAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryUpdateAsync(TenantInfo tenantInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
