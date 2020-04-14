using AlonsoAdmin.MultiTenant.Auth;
using AlonsoAdmin.MultiTenant.Builder;
using AlonsoAdmin.MultiTenant.Store;
using AlonsoAdmin.MultiTenant.Strategy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Extensions
{
    public static class MultiTenantBuilderExtensions
    {
        public static MultiTenantBuilder WithRemoteAuthentication(this MultiTenantBuilder builder)
        {
            // 替换验证的实现实例
            builder.Services.Replace(ServiceDescriptor.Singleton<IAuthenticationSchemeProvider, MultiTenantAuthenticationSchemeProvider>());
            // builder.Services.Replace(ServiceDescriptor.Scoped<IAuthenticationService, MultiTenantAuthenticationService>());
            builder.Services.DecorateService<IAuthenticationService, MultiTenantAuthenticationService>();
            builder.Services.TryAddSingleton<RemoteAuthenticationStrategy>();

            return builder;
        }

        public static MultiTenantBuilder WithBasePathStrategy(this MultiTenantBuilder builder)
            => builder.WithStrategy<BasePathStrategy>(ServiceLifetime.Singleton);

        /// <summary>
        /// 启用默认路由策略  route parameter 默认为 "__tenant__" 
        /// </summary>
        /// <returns>The same MultiTenantBuilder passed into the method.</returns>
        public static MultiTenantBuilder WithRouteStrategy(this MultiTenantBuilder builder)
            => builder.WithRouteStrategy("__tenant__");

        /// <summary>
        /// 启用制定路由策略
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="tenantParam"></param>
        /// <returns></returns>
        public static MultiTenantBuilder WithRouteStrategy(
            this MultiTenantBuilder builder,
            string tenantParam)
        {
            if (string.IsNullOrWhiteSpace(tenantParam))
            {
                throw new ArgumentException("\"tenantParam\" 不能为空", nameof(tenantParam));
            }
            return builder.WithStrategy<RouteStrategy>(ServiceLifetime.Singleton, new object[] { tenantParam });
        }


        /// <summary>
        /// 启用默认路由策略 "__tenant__.*"
        /// </summary>
        /// <returns>The same MultiTenantBuilder passed into the method.</returns>
        public static MultiTenantBuilder WithHostStrategy(this MultiTenantBuilder builder)
            => builder.WithHostStrategy("__tenant__.*");

        /// <summary>
        /// Adds and configures a HostStrategy to the application.
        /// </summary>
        /// <param name="template">The template for determining the tenant identifier in the host.</param>
        /// <returns>The same MultiTenantBuilder passed into the method.</returns>
        public static MultiTenantBuilder WithHostStrategy(
            this MultiTenantBuilder builder,
            string template)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentException("Invalid value for \"template\"", nameof(template));
            }

            return builder.WithStrategy<HostStrategy>(ServiceLifetime.Singleton, new object[] { template });
        }



        #region 获取远程租户参数
        /// <summary>
        /// 获取远程租户参数
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="endpointTemplate"></param>
        /// <returns></returns>
        public static MultiTenantBuilder WithHttpRemoteStore(
            this MultiTenantBuilder builder,
            string endpointTemplate)
            => builder.WithHttpRemoteStore(endpointTemplate, null);

        /// <summary>
        /// 获取远程租户参数
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="endpointTemplate"></param>
        /// <param name="clientConfig"></param>
        /// <returns></returns>
        public static MultiTenantBuilder WithHttpRemoteStore(
            this MultiTenantBuilder builder,
            string endpointTemplate,
            Action<IHttpClientBuilder> clientConfig)
        {
            var httpClientBuilder = builder.Services.AddHttpClient(typeof(HttpRemoteStoreClient).FullName);
            if (clientConfig != null) clientConfig(httpClientBuilder);

            builder.Services.TryAddSingleton<HttpRemoteStoreClient>();
            return builder.WithStore<HttpRemoteStore>(ServiceLifetime.Singleton, endpointTemplate);
        }
        #endregion

        #region 获取配置文件参数
        /// <summary>
        /// 配置文件参数存储
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MultiTenantBuilder WithConfigurationStore(this MultiTenantBuilder builder)
            => builder.WithStore<ConfigurationStore>(ServiceLifetime.Singleton);

        /// <summary>
        /// 配置文件参数存储
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <param name="sectionName">节点名称</param>
        /// <returns></returns>
        public static MultiTenantBuilder WithConfigurationStore(
            this MultiTenantBuilder builder,
            IConfiguration configuration,
            string sectionName)
            => builder.WithStore<ConfigurationStore>(ServiceLifetime.Singleton, configuration, sectionName);

        #endregion

        #region 启用静态策略
        /// <summary>
        /// 静态策略
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="identifier">策略标识</param>
        /// <returns></returns>
        public static MultiTenantBuilder WithStaticStrategy(
            this MultiTenantBuilder builder,
            string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("Invalid value for \"identifier\"", nameof(identifier));
            }

            return builder.WithStrategy<StaticStrategy>(ServiceLifetime.Singleton, new object[] { identifier }); ;
        }
        #endregion

        #region 启用委托策略
        /// <summary>
        /// 委托策略
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="doStrategy">委托函数</param>
        /// <returns></returns>
        public static MultiTenantBuilder WithDelegateStrategy(
            this MultiTenantBuilder builder,
            Func<object, Task<string>> doStrategy)
        {
            if (doStrategy == null)
            {
                throw new ArgumentNullException(nameof(doStrategy));
            }
            return builder.WithStrategy<DelegateStrategy>(ServiceLifetime.Singleton, new object[] { doStrategy });
        }
        #endregion
    }
}
