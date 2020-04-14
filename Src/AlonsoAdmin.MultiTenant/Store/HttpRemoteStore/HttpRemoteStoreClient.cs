using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Store
{
    public class HttpRemoteStoreClient
    {
        private readonly IHttpClientFactory clientFactory;

        public HttpRemoteStoreClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<TenantInfo> TryGetByCodeAsync(string endpointTemplate, string code)
        {
            var client = clientFactory.CreateClient(typeof(HttpRemoteStoreClient).FullName);
            var uri = endpointTemplate.Replace(HttpRemoteStore.defaultEndpointTemplateIdentifierToken, code);
            var response = await client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            // var anon = new { Id = "", Code = "", Name = "", ConnectionString = "" };

            var anon = new TenantInfo();
            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var result = JsonConvert.DeserializeAnonymousType(json, anon);
            return result;
        }

    }
}
