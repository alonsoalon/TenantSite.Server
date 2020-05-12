using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services
{
    public interface ISettingService
    {
        Task<SystemConfig> GetSystemSettingsAsync();
        Task<StartupConfig> GetStartupSettingsAsync();
        Task<List<MultiTenant.TenantInfo>> GetTenantListAsync();


        bool WriteConfig<T>(string node, Action<T> applyChanges);

        bool WriteTenantsConfig(List<TenantInfo> tenants);

    }
}
