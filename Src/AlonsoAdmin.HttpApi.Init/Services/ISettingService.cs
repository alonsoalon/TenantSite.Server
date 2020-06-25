using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Init.Services
{
    public interface ISettingService
    {
        /// <summary>
        /// 得到系统参数
        /// </summary>
        /// <returns></returns>
        Task<SystemConfig> GetSystemSettingsAsync();

        /// <summary>
        /// 得到启动参数
        /// </summary>
        /// <returns></returns>
        Task<StartupConfig> GetStartupSettingsAsync();

        /// <summary>
        /// 得到组合配置
        /// </summary>
        /// <returns></returns>
        Task<List<MultiTenant.TenantInfo>> GetTenantListAsync();

        /// <summary>
        /// 写配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="applyChanges"></param>
        /// <returns></returns>
        Task<bool> WriteConfig<T>(string node, Action<T> applyChanges);

        /// <summary>
        /// 写租户配置
        /// </summary>
        /// <param name="tenants"></param>
        /// <returns></returns>
        Task<bool> WriteTenantsConfig(List<TenantInfo> tenants);

    }
}
