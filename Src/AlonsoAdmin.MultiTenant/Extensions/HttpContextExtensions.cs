using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 返回当前的MultiTenantContext，如果没有，则返回null。
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static MultiTenantContext GetMultiTenantContext(this HttpContext httpContext)
        {
            object multiTenantContext = null;
            httpContext.Items.TryGetValue(Constants.HttpContextMultiTenantContext, out multiTenantContext);

            return (MultiTenantContext)multiTenantContext;
        }

        /// <summary>
        /// 在MultiTenantContext中设置租户信息TenantInfo。
        /// 将多租户上下文中的StrategyInfo和StoreInfo设置为null。
        /// 可选地重置当前依赖项注入服务提供者。
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="tenantInfo"></param>
        /// <param name="resetServiceProvider"></param>
        /// <returns></returns>
        public static bool TrySetTenantInfo(this HttpContext httpContext, TenantInfo tenantInfo, bool resetServiceProvider)
        {
            var multitenantContext = httpContext.GetMultiTenantContext();

            if (multitenantContext == null)
                return false;

            if (resetServiceProvider)
                httpContext.RequestServices = httpContext.RequestServices.CreateScope().ServiceProvider;

            multitenantContext.TenantInfo = tenantInfo;
            multitenantContext.StrategyInfo = null;
            multitenantContext.StoreInfo = null;

            return true;
        }
    }
}
