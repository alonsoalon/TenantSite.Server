using AlonsoAdmin.MultiTenant.Error;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class MultiTenantStrategyWrapper<TStrategy> : IMultiTenantStrategy
        where TStrategy : IMultiTenantStrategy
    {
        public TStrategy Strategy { get; }

        private readonly ILogger logger;

        public MultiTenantStrategyWrapper(TStrategy strategy, ILogger<TStrategy> logger)
        {
            this.Strategy = strategy;
            this.logger = logger;
        }


        public async Task<string> GetIdentifierAsync(object context)
        {
            if (context == null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            string id = null;

            try
            {
                id = await Strategy.GetIdentifierAsync(context);
            }
            catch (Exception e)
            {
                var errorMessage = $"在{typeof(TStrategy)}.GetIdentifierAsync 出错";                
                logger.LogError(errorMessage);
                throw new MultiTenantException(errorMessage, e);
            }

            if (id != null)
            {
                logger.LogInformation($"{typeof(TStrategy)}.GetIdentifierAsync: 找到租户: \"{id}\".");
            }
            else
            {
                logger.LogInformation($"{typeof(TStrategy)}.GetIdentifierAsync: 未找到租户.");
            }

            return id;
        }
    }
}
