using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;

namespace AlonsoAdmin.HttpApi
{
    public static class AutoMapperExtensions
    {
        public static void RegisterMapper(this IServiceCollection services)
        {

            var assemblyName = "AlonsoAdmin.Services";
            // 根据程序集的名字获取程序集对象
            Assembly assembly = Assembly.Load(assemblyName);//var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            //根据程序集的名字 获取程序集中所有的类型
            //var ServiceDll = Path.Combine(basePath, "Admin.Core.Service.dll");
            //var assembly = Assembly.LoadFrom(ServiceDll);

            services.AddAutoMapper(assembly);
        }


    }
}
