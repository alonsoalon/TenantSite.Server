using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.HttpApi.Auth;
using AlonsoAdmin.HttpApi.AuthStore;
using AlonsoAdmin.Repository;
using AlonsoAdmin.Repository.System;
using AlonsoAdmin.Services.System;
using FreeSql;
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
            services.RegsiterDomains(env);
            services.RegsiterServices(env);

        }

        /// <summary>
        /// 注册所有模块的services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        /// <param name="serviceLifetime"></param>
        private static void RegsiterServices(this IServiceCollection services, IWebHostEnvironment env = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var assemblyName = "AlonsoAdmin.Services";
            // 根据程序集的名字获取程序集对象
            Assembly assembly = Assembly.Load(assemblyName);//var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            //根据程序集的名字 获取程序集中所有的类型
            IEnumerable<Type> types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.FullName.EndsWith("Service"));
            foreach (var implType in types)
            {
                var interfaceList = implType.GetInterfaces().Where(x => x.Name.EndsWith(implType.Name));
                if (interfaceList.Any())
                {
                    var interType = interfaceList.First();
                    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interType, implType, serviceLifetime);
                    services.Add(serviceDescriptor);
                }
            }
        }


        private static void RegsiterDomains(this IServiceCollection services, IWebHostEnvironment env = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) {
            var assemblyName = "AlonsoAdmin.Domain"; 
             Assembly assembly = Assembly.Load(assemblyName);
            //Type[] types = assembly.GetTypes();
            //根据程序集的名字 获取程序集中Domain类
            IEnumerable<Type> domianTypes = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.FullName.EndsWith("Domain"));

            foreach (var implType in domianTypes)
            {
   
                var interfaceList = implType.GetInterfaces().Where(x => x.Name.EndsWith(implType.Name));
                if (interfaceList.Any()) {       
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
        /// <param name="env"></param>
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

            //services.AddScoped<UnitOfWorkManager>(x => new UowManager(x.GetRequiredService<IMultiTenantDbFactory>(), Constants.SystemDbKey));
            //services.AddScoped<UnitOfWorkManager>(x => new UowManager(x.GetRequiredService<IMultiTenantDbFactory>(), Constants.BlogDbKey));
            //services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IMultiTenantDbFactory>().Db(Constants.SystemDbKey).CreateUnitOfWork());
            //services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IMultiTenantDbFactory>().Db(Constants.BlogDbKey).CreateUnitOfWork());
            //services.AddScoped(implementationFactory =>
            //{
            //    Func<string, UowManager> accesor = key =>
            //    {
                    
            //        if (key.Equals(Constants.SystemDbKey))
            //        {
            //            return new UowManager(implementationFactory.GetRequiredService<IMultiTenantDbFactory>(), Constants.SystemDbKey);
            //        }
            //        else if (key.Equals(Constants.BlogDbKey))
            //        {
            //            return new UowManager(implementationFactory.GetRequiredService<IMultiTenantDbFactory>(), Constants.BlogDbKey);
            //        }
            //        else
            //        {
            //            throw new ArgumentException($"Not Support key : {key}");
            //        }
            //    };
            //    return accesor;
            //});


            var assemblyName = "AlonsoAdmin.Repository";
            Assembly assembly = Assembly.Load(assemblyName); // var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            //根据程序集的名字 获取程序集中所有的类型
            Type[] types = assembly.GetTypes();

            //IEnumerable<Type> uowManagerTypes = types.Where(t =>
            //     t.IsClass &&
            //     !t.IsAbstract &&
            //     t.FullName.EndsWith("UowManager")
            //);

            //// 注册各个模块工作单元管理器
            ////services.AddScoped<IUowManager, Repository.System.UowManager>();
            //foreach (var implType in uowManagerTypes)
            //{
            //    //services.AddScoped<IUowManager, Repository.System.UowManager>();
            //    ServiceDescriptor uowManagerServiceDescriptor = new ServiceDescriptor(typeof(UnitOfWorkManager), implType, ServiceLifetime.Scoped);
            //    services.Add(uowManagerServiceDescriptor);
            //}



            //过滤上述程序集 得到仓储实现类
            IEnumerable<Type> _types = types.Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.FullName.EndsWith("Repository")
            );



            foreach (var implType in _types)
            {

                var interfaceList = implType.GetInterfaces().Where(x => x.Name.EndsWith(implType.Name));
                if (interfaceList.Any())
                {
                    var interType = interfaceList.First();
                    //var interFullName = $"{implType.Namespace}.I{implType.Name}";
                    //var interType = assembly.GetType(interFullName);
                    

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
