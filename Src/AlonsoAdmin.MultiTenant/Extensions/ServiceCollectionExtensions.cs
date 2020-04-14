using AlonsoAdmin.MultiTenant.Accessor;
using AlonsoAdmin.MultiTenant.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static MultiTenantBuilder AddMultiTenant(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddScoped<TenantInfo>(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.GetMultiTenantContext()?.TenantInfo);
            services.TryAddSingleton<IMultiTenantContextAccessor, MultiTenantContextAccessor>();

            return new MultiTenantBuilder(services);
        }


        /// <summary>
        /// 装载服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="services"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool DecorateService<TService, TImpl>(this IServiceCollection services, params object[] parameters)
        {
            var existingService = services.SingleOrDefault(s => s.ServiceType == typeof(TService));
            if (existingService == null)
                return false;

            var newService = new ServiceDescriptor(
                existingService.ServiceType,
                sp =>
                {
                    TService inner = (TService)ActivatorUtilities.CreateInstance(sp, existingService.ImplementationType);

                    var parameters2 = new object[parameters.Length + 1];
                    Array.Copy(parameters, 0, parameters2, 1, parameters.Length);
                    parameters2[0] = inner;

                    return ActivatorUtilities.CreateInstance<TImpl>(sp, parameters2);
                },
                existingService.Lifetime);

            if (existingService.ImplementationInstance != null)
            {
                newService = new ServiceDescriptor(
                    existingService.ServiceType,
                    sp =>
                    {
                        TService inner = (TService)existingService.ImplementationInstance;
                        return ActivatorUtilities.CreateInstance<TImpl>(sp, inner, parameters);
                    },
                    existingService.Lifetime);
            }
            else if (existingService.ImplementationFactory != null)
            {
                newService = new ServiceDescriptor(
                    existingService.ServiceType,
                    sp =>
                    {
                        TService inner = (TService)existingService.ImplementationFactory(sp);
                        return ActivatorUtilities.CreateInstance<TImpl>(sp, inner, parameters);
                    },
                    existingService.Lifetime);
            }

            services.Remove(existingService);
            services.Add(newService);

            return true;
        }
    }
}
