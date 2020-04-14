using AlonsoAdmin.MultiTenant.Store;
using AlonsoAdmin.MultiTenant.Strategy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Middleware
{
    public class MultiTenantMiddleware
    {
        private readonly RequestDelegate next;

        public MultiTenantMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

       
        public async Task InvokeAsync(HttpContext context)
        {
            
            // 多租户上下文设置到项集合中。
            if (!context.Items.ContainsKey(Constants.HttpContextMultiTenantContext))
            {
                var multiTenantContext = new MultiTenantContext();
                context.Items.Add(Constants.HttpContextMultiTenantContext, multiTenantContext);

                IMultiTenantStrategy strategy = null;
                string identifier = null;

                // 取回IMultiTenantStrategy实例 循环 直到取到租户结束
                foreach (var strat in context.RequestServices.GetServices<IMultiTenantStrategy>())
                {
                    // 取回租户ID
                    identifier = await strat.GetIdentifierAsync(context);
                    if (identifier != null)
                    {
                        strategy = strat;
                        break;
                    }
                }

                var store = context.RequestServices.GetRequiredService<IMultiTenantStore>();
                TenantInfo tenantInfo = null;
                if (identifier != null)
                {
                    SetStrategyInfo(multiTenantContext, strategy);
                    tenantInfo = await store.TryGetByCodeAsync(identifier);
                }
                // 解决远程身份验证回调(如果适用)。
                if (tenantInfo == null)
                {
                    strategy = context.RequestServices.GetService<RemoteAuthenticationStrategy>();

                    if (strategy != null)
                    {
                        identifier = await strategy.GetIdentifierAsync(context);
                        if (identifier != null)
                        {
                            SetStrategyInfo(multiTenantContext, strategy);
                            tenantInfo = await store.TryGetByIdAsync(identifier);
                        }
                    }
                }

                // Finally try the fallback identifier, if applicable.
                // 最后，如果适用，尝试fallback。
                if (tenantInfo == null)
                {
                    //strategy = context.RequestServices.GetService<FallbackStrategy>();
                    strategy = context.RequestServices.GetService<StaticStrategy>();
                    
                    if (strategy != null)
                    {
                        identifier = await strategy.GetIdentifierAsync(context);
                        SetStrategyInfo(multiTenantContext, strategy);
                        tenantInfo = await store.TryGetByIdAsync(identifier);
                    }
                }

                if (tenantInfo != null)
                {
                    // 设置租户信息
                    multiTenantContext.TenantInfo = tenantInfo;
                    tenantInfo.MultiTenantContext = multiTenantContext;

                    // 设置租户存储信息
                    var storeInfo = new StoreInfo();
                    storeInfo.MultiTenantContext = multiTenantContext;
                    storeInfo.Store = store;
                    if (store.GetType().IsGenericType &&
                        store.GetType().GetGenericTypeDefinition() == typeof(MultiTenantStoreWrapper<>))
                    {
                        storeInfo.Store = (IMultiTenantStore)store.GetType().GetProperty("Store").GetValue(store);
                        storeInfo.StoreType = store.GetType().GetGenericArguments().First();
                    }
                    else
                    {
                        storeInfo.Store = store;
                        storeInfo.StoreType = store.GetType();
                    }
                    multiTenantContext.StoreInfo = storeInfo;
                }
            }

            if (next != null)
            {
                await next(context);
            }
        }

        private static void SetStrategyInfo(MultiTenantContext multiTenantContext, IMultiTenantStrategy strategy)
        {
            var strategyInfo = new StrategyInfo();
            strategyInfo.MultiTenantContext = multiTenantContext;
            strategyInfo.Strategy = strategy;
            if (strategy.GetType().IsGenericType &&
                strategy.GetType().GetGenericTypeDefinition() == typeof(MultiTenantStrategyWrapper<>))
            {
                strategyInfo.Strategy = (IMultiTenantStrategy)strategy.GetType().GetProperty("Strategy").GetValue(strategy);
                strategyInfo.StrategyType = strategy.GetType().GetGenericArguments().First();
            }
            else
            {
                strategyInfo.Strategy = strategy;
                strategyInfo.StrategyType = strategy.GetType();
            }
            multiTenantContext.StrategyInfo = strategyInfo;
        }
    }
}
