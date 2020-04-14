using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using AlonsoAdmin.MultiTenant.Options;
using AlonsoAdmin.MultiTenant.Store;
using AlonsoAdmin.MultiTenant.Strategy;

namespace AlonsoAdmin.MultiTenant.Builder
{

    public class MultiTenantBuilder
    {
        public IServiceCollection Services { get; set; }

        public MultiTenantBuilder(IServiceCollection services)
        {
            Services = services;
        }


        /// <summary>
        /// 为每个租户添加配置参数。
        /// </summary>
        /// <typeparam name="TOptions">配置参数</typeparam>
        /// <param name="tenantInfo">租户信息</param>
        /// <returns></returns>
        public MultiTenantBuilder WithPerTenantOptions<TOptions>(Action<TOptions, TenantInfo> tenantInfo) where TOptions : class, new()
        {
            if (tenantInfo == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo));
            }

            Services.TryAddSingleton<IOptionsMonitorCache<TOptions>>(sp =>
            {
                return (MultiTenantOptionsCache<TOptions>)ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsCache<TOptions>));
            });

            // Necessary to apply tenant options in between configuration and postconfiguration
            // 应用在configuration 与 postconfiguration 之间
            Services.TryAddTransient<IOptionsFactory<TOptions>>(sp =>
            {
                return (IOptionsFactory<TOptions>)ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsFactory<TOptions>), new[] { tenantInfo });
            });
            // 注册为为当前请求生命周期生效实例
            Services.TryAddScoped<IOptionsSnapshot<TOptions>>(sp => BuildOptionsManager<TOptions>(sp));
            // 注册为单例
            Services.TryAddSingleton<IOptions<TOptions>>(sp => BuildOptionsManager<TOptions>(sp));

            return this;
        }


        /// <summary>
        /// 得到实例
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="sp"></param>
        /// <returns></returns>
        private static MultiTenantOptionsManager<TOptions> BuildOptionsManager<TOptions>(IServiceProvider sp) where TOptions : class, new()
        {
            var cache = ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsCache<TOptions>));
            return (MultiTenantOptionsManager<TOptions>)ActivatorUtilities.CreateInstance(sp, typeof(MultiTenantOptionsManager<TOptions>), new[] { cache });
        }

        /// <summary>
        /// 构造Store实例
        /// </summary>
        /// <typeparam name="TStore">存储类 类型</typeparam>
        /// <param name="lifetime">生命周期</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回当前类实例</returns>
        public MultiTenantBuilder WithStore<TStore>(ServiceLifetime lifetime, params object[] parameters) where TStore : IMultiTenantStore
        {
            return WithStore<TStore>(lifetime, sp => ActivatorUtilities.CreateInstance<TStore>(sp, parameters));
        }

        /// <summary>
        /// 构造Store实例
        /// </summary>
        /// <typeparam name="TStore">存储类 类型</typeparam>
        /// <param name="lifetime"></param>
        /// <param name="factory">工厂</param>
        /// <returns>返回当前类实例</returns>
        public MultiTenantBuilder WithStore<TStore>(ServiceLifetime lifetime, Func<IServiceProvider, TStore> factory) where TStore : IMultiTenantStore
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            Services.TryAdd(ServiceDescriptor.Describe(typeof(IMultiTenantStore), sp => new MultiTenantStoreWrapper<TStore>(factory(sp), sp.GetService<ILogger<TStore>>()), lifetime));

            return this;
        }
        /// <summary>
        /// 使用默认的依赖项注入向应用程序添加策略
        /// </summary>
        /// <typeparam name="TStrategy">策略类 类型</typeparam>
        /// <param name="lifetime">生命周期</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回当前类实例</returns>
        public MultiTenantBuilder WithStrategy<TStrategy>(ServiceLifetime lifetime, params object[] parameters) where TStrategy : IMultiTenantStrategy
            => WithStrategy(lifetime, sp => ActivatorUtilities.CreateInstance<TStrategy>(sp, parameters));

        /// <summary>
        /// 使用工厂方法向应用程序添加和配置IMultiTenantStrategy。
        /// </summary>
        /// <typeparam name="TStrategy">策略类 类型</typeparam>
        /// <param name="lifetime">生命周期</param>
        /// <param name="factory">工厂</param>
        /// <returns>返回当前类实例</returns>
        public MultiTenantBuilder WithStrategy<TStrategy>(ServiceLifetime lifetime, Func<IServiceProvider, TStrategy> factory)
            where TStrategy : IMultiTenantStrategy
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            Services.Add(ServiceDescriptor.Describe(typeof(IMultiTenantStrategy),
                sp => new MultiTenantStrategyWrapper<TStrategy>(factory(sp), sp.GetService<ILogger<TStrategy>>()), lifetime));

            return this;
        }

    }
}
