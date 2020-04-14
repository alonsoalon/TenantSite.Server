using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Store
{
    public interface IMultiTenantStore
    {

        /// <summary>
        /// 尝试将TenantInfo添加到存储中。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> TryAddAsync(TenantInfo tenantInfo);

        /// <summary>
        /// 尝试更新存储中的TenantInfo。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<bool> TryUpdateAsync(TenantInfo tenantInfo);

        /// <summary>
        /// 尝试从存储中删除TenantInfo。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> TryRemoveAsync(string id);

        /// <summary>
        /// 检索指定ID的TenantInfo。
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        Task<TenantInfo> TryGetByIdAsync(string id);

        /// <summary>
        /// 检索指定Code的TenantInfo。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TenantInfo> TryGetByCodeAsync(string id);
    }
}
