using AlonsoAdmin.MultiTenant.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Store
{
    public class ConfigurationStore: IMultiTenantStore
    {
        internal const string defaultSectionName = "Tenants";
        private readonly IConfigurationSection section;
        private IEnumerable<TenantInfo> tenantMap;


        public ConfigurationStore(IConfiguration configuration) : this(configuration, defaultSectionName)
        {
        }

        public ConfigurationStore(IConfiguration configuration, string sectionName)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException("配置节点名不能为空", nameof(sectionName));
            }

            section = configuration.GetSection(sectionName);
            if (!section.Exists())
            {
                throw new MultiTenantException("无效的配置节点");
            }

            //更新租户参数
            UpdateTenantMap();

            //监控section节点是否有变化，有变化触发UpdateTenantMap
            ChangeToken.OnChange(() => section.GetReloadToken(), UpdateTenantMap);
        }

        private void UpdateTenantMap()
        {
            var tenants = section.Get<IEnumerable<TenantInfo>>();
            tenantMap = tenants;
        }

        public async Task<TenantInfo> TryGetByIdAsync(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await Task.FromResult(tenantMap.Where(x => x.Id.ToLower() == id.ToLower()).SingleOrDefault());
        }

        public async Task<TenantInfo> TryGetByCodeAsync(string code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            return await Task.FromResult(tenantMap.Where(x => x.Code.ToLower() == code.ToLower()).SingleOrDefault());
        }

        public Task<bool> TryAddAsync(TenantInfo tenantInfo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryRemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryUpdateAsync(TenantInfo tenantInfo)
        {
            throw new NotImplementedException();
        }
    }
}
