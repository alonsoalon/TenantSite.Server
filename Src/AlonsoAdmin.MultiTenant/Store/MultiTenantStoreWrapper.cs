using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Store
{
    public class MultiTenantStoreWrapper<TStore> : IMultiTenantStore
        where TStore : IMultiTenantStore
    {
        public TStore Store { get; }
        private readonly ILogger logger;

        public MultiTenantStoreWrapper(TStore store, ILogger<TStore> logger)
        {
            this.Store = store;
            this.logger = logger;
        }

        public async Task<TenantInfo> TryGetByIdAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            TenantInfo result = null;

            try
            {
                result = await Store.TryGetByIdAsync(id);
            }
            catch (Exception e)
            {
                var errorMessage = $"异常： {typeof(TStore)}.TryGetByIdAsync.";
                logger.LogError(errorMessage,e);
            }

            if (result != null)
            {
                logger.LogInformation($"{typeof(TStore)}.TryGetByIdAsync: 找到租户: \"{id}\".");
            }
            else
            {
                logger.LogInformation($"{typeof(TStore)}.TryGetByIdAsync: 未找到租户: \"{id}\".");
            }

            return result;
        }

        public async Task<TenantInfo> TryGetByCodeAsync(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            TenantInfo result = null;

            try
            {
                result = await Store.TryGetByCodeAsync(code);
            }
            catch (Exception e)
            {
                var errorMessage = $"出错： {typeof(TStore)}.TryGetByCodeAsync.";
                logger.LogError(errorMessage, e);
            }

            if (result != null)
            {
                logger.LogInformation($"{typeof(TStore)}.TryGetByCodeAsync: 找到租户: \"{code}\".");
            }
            else
            {
                logger.LogInformation($"{typeof(TStore)}.TryGetByCodeAsync: 未找到租户: \"{code}\".");
                
            }

            return result;
        }

        public async Task<bool> TryAddAsync(TenantInfo tenantInfo)
        {
            if (tenantInfo == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo));
            }

            if (tenantInfo.Id == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo.Id));
            }

            if (tenantInfo.Code == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo.Code));
            }

            try
            {
                
                var existingById = await TryGetByIdAsync(tenantInfo.Id); //通过ID获取租户
                var existingByCode = await TryGetByCodeAsync(tenantInfo.Code); //通过Code获取租户
                if (existingById != null)
                {
                    logger.LogInformation($"{typeof(TStore)}.TryAddAsync: 租户已存在. Id: \"{tenantInfo.Id}\", Code: \"{tenantInfo.Code}\"");
                    return false;
                }

                if (existingByCode != null)
                {
                    logger.LogInformation($"{typeof(TStore)}.TryAddAsync: 租户已存在. Id: \"{tenantInfo.Id}\", Code: \"{tenantInfo.Code}\"");
                    return false;
                }

                var result = await Store.TryAddAsync(tenantInfo);

                if (result)
                {
                    logger.LogInformation($"{typeof(TStore)}.TryAddAsync: 租户添加成功. Id: \"{tenantInfo.Id}\", Code: \"{tenantInfo.Code}\"");
                }
                else
                {
                    logger.LogInformation($"{typeof(TStore)}.TryAddAsync: 租户添加失败. Id: \"{tenantInfo.Id}\", Code: \"{tenantInfo.Code}\"");
                }
                return result;

            }
            catch (Exception e)
            {
                var errorMessage = $"Exception in {typeof(TStore)}.TryAddAsync.";
                logger.LogError(errorMessage, e);
                return false;
            }
        }

        public async Task<bool> TryUpdateAsync(TenantInfo tenantInfo)
        {
            if (tenantInfo == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo));
            }

            if (tenantInfo.Id == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo.Id));
            }

            try
            {
                var existing = await TryGetByIdAsync(tenantInfo.Id);
                if (existing == null)
                {
                    logger.LogInformation($"{typeof(TStore)}.TryUpdateAsync: 没有找到租户: \"{tenantInfo.Id}\" ");
                    return false;
                }

                var result = await Store.TryUpdateAsync(tenantInfo);

                if (result)
                {
                    logger.LogInformation($"{typeof(TStore)}.TryUpdateAsync: 租户: \"{tenantInfo.Id}\" 更新成功");
                }
                else
                {
                    logger.LogInformation($"{typeof(TStore)}.TryUpdateAsync: 租户: \"{tenantInfo.Id}\" 更新失败");
                }

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Exception in {typeof(TStore)}.TryUpdateAsync.";
                logger.LogError(errorMessage, e);
                return false;
            }
        }

        public async Task<bool> TryRemoveAsync(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = false;

            try
            {
                result = await Store.TryRemoveAsync(id);
            }
            catch (Exception e)
            {
                var errorMessage = $"异常 {typeof(TStore)}.TryRemoveAsync.";
                logger.LogError(errorMessage, e);
            }

            if (result)
            {
                logger.LogInformation($"{typeof(TStore)}.TryRemoveAsync: 租户: \"{id}\" 被移除");
            }
            else
            {
                logger.LogInformation($"{typeof(TStore)}.TryRemoveAsync: 不能移除租户: \"{id}\" ");
            }

            return result;
        }


    }
}
