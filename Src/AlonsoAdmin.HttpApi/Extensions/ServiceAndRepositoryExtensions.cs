using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.HttpApi.Auth;
using AlonsoAdmin.HttpApi.AuthStore;
using AlonsoAdmin.Repository;
using AlonsoAdmin.Services.System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi
{
    public static class ServiceAndRepositoryExtensions
    {
        /// <summary>
        /// 注册所有模块的services and Repositories
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public static void RegsiterServicesAndRepositories(this IServiceCollection services, IWebHostEnvironment env) {

            services.RegsiterRepositories(env);
            services.RegsiterServices(env);

        }

        /// <summary>
        /// 注册所有模块的services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        private static void RegsiterServices(this IServiceCollection services, IWebHostEnvironment env = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var assemblyName = "AlonsoAdmin.Services";
            // 根据程序集的名字获取程序集对象
            Assembly assembly = Assembly.Load(assemblyName);//var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            //根据程序集的名字 获取程序集中所有的类型
            Type[] types = assembly.GetTypes();

            var list = types.Where(t =>
            t.IsClass && 
            !t.IsAbstract && 
            !t.IsGenericType &&
            !t.FullName.Contains("<")
            ).ToList();

            foreach (var implType in list)
            {
                var interfaceList = implType.GetInterfaces();
                if (interfaceList.Any())
                {
                    var interType = interfaceList.First();
                    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interType, implType, serviceLifetime);
                    services.Add(serviceDescriptor);
                }
            }
        }

        /// <summary>
        /// 注册所有模块的Repository
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        private static void RegsiterRepositories(this IServiceCollection services, IWebHostEnvironment env = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {


            var ib = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(10));
            ib.Notice += (_, e) =>
            {
                if (env != null && env.IsDevelopment())
                {
                    //if (e.NoticeType == IdleBus<IFreeSql>.NoticeType.AutoRelease)
                    //_logger.LogInformation($"[{DateTime.Now.ToString("g")}] {e.Key} 空闲被回收");
                }
            };
            // FreeSql 实例存储器ib 加入容器
            services.AddSingleton(ib);

            // 多租户数据工厂添加到容器
            services.AddSingleton<IMultiTenantDbFactory, MultiTenantDbFactory>();
            var assemblyName = "AlonsoAdmin.Repository";
            Assembly assembly = Assembly.Load(assemblyName); // var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            //根据程序集的名字 获取程序集中所有的类型
            Type[] types = assembly.GetTypes();
            //过滤上述程序集 首先按照传进来的条件进行过滤 接着要求Type必须是类，而且不能是抽象类
            IEnumerable<Type> _types = types.Where(t =>
            t.IsClass &&
            !t.IsAbstract &&
            !t.FullName.StartsWith($"{assemblyName}.MultiTenantDbFactory") &&
            !t.FullName.StartsWith($"{assemblyName}.RepositoryBase") &&
            //!t.FullName.EndsWith("<>c") &&
            !t.FullName.Contains("<")
            );

            foreach (var implType in _types)
            {
                var name = implType.FullName;
                var name2 = implType.Name;
                var interfaceList = implType.GetInterfaces();
                if (interfaceList.Any())
                {
                    //var interType = interfaceList.First();
                    var interFullName = $"{implType.Namespace}.I{implType.Name}";
                    var interType = assembly.GetType(interFullName);
                    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interType, implType, serviceLifetime);
                    services.Add(serviceDescriptor);

                    //switch (serviceLifetime)
                    //{
                    //    case ServiceLifetime.Transient:
                    //        services.AddTransient(interType, implType);
                    //        break;
                    //    case ServiceLifetime.Scoped:
                    //        services.AddScoped(interType, implType);
                    //        break;
                    //    case ServiceLifetime.Singleton:
                    //        services.AddSingleton(interType, implType);
                    //        break;
                    //}
                }
            }
        }


    }
}
