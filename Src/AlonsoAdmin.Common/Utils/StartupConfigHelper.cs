using AlonsoAdmin.Common.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlonsoAdmin.Common.Utils
{
    /// <summary>
    /// 用于在静态方法中获取配置文件的程序启动参数，在非静态方法中应用微软的Options相关方法获取
    /// 故在此写死了只获取Startup节点内容
    /// </summary>
    public static class StartupConfigHelper
    {
        static IConfiguration Configuration;
        static StartupConfigHelper()
        {
            // 这里读取环境变量
            var provider = new EnvironmentVariablesConfigurationProvider();
            provider.Load();
            provider.TryGet("ASPNETCORE_ENVIRONMENT", out string environmentName);

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
           
        }

        public static StartupConfig Get()
        {
            return Configuration.GetSection("Startup").Get<StartupConfig>();
        }

        //public static string GetValue(string key)
        //{
        //    return Configuration[key];
        //}

        //public static T GetValue<T>(string key)
        //{
        //    return Configuration.GetValue<T>(key);
        //}
    }
}
