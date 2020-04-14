using AlonsoAdmin.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;


namespace AlonsoAdmin.Common.Utils
{
    /// <summary>
    /// 设置参数帮助类
    /// T config = SettingHelper.Get<T>("appconfig", env.EnvironmentName,"configs") ?? new T();
    /// </summary>
    public static class SettingHelper
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static IConfiguration Load(string fileName, string environmentName = "",string baseDir="configs", bool reloadOnChange = false)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, baseDir);
            if (!Directory.Exists(filePath))
                return null;

            var builder = new ConfigurationBuilder()
                .SetBasePath(filePath)
                .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

            if (environmentName.IsNotNull())
            {
                builder.AddJsonFile(fileName.ToLower() + "." + environmentName + ".json", true, reloadOnChange);
            }

            return builder.Build();
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <typeparam name="T">配置信息</typeparam>
        /// <param name="fileName"></param>
        /// <param name="environmentName">文件名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static T Get<T>(string fileName, string environmentName = "", string baseDir = "configs", bool reloadOnChange = false)
        {
            var configuration = Load(fileName, environmentName, baseDir, reloadOnChange);
            if (configuration == null)
                return default;

            return configuration.Get<T>();
        }

     
    }
}
