using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Utils
{
    public static class SettingHelper
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static IConfiguration Load(string fileName, string dir = "", string environmentName = "", bool reloadOnChange = false)
        {
            string filePath = "";
            var reg = new Regex(@"^[A-Z]+:\\");
            if (dir != "" && reg.Match(dir).Success)
            {
                filePath = dir;
            }
            else {
                filePath = AppContext.BaseDirectory;
            }

            if (!Directory.Exists(dir))
                return null;

            var builder = new ConfigurationBuilder()
                .SetBasePath(filePath)
                .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

            if (!string.IsNullOrWhiteSpace(environmentName))
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
        public static T Get<T>(string fileName, string dir = "", string environmentName = "", bool reloadOnChange = false)
        {
            var configuration = Load(fileName, dir, environmentName, reloadOnChange);
            if (configuration == null)
                return default;

            

            return configuration.Get<T>();
        }


        //public static string Write(string fileName, string environmentName = "", string baseDir = "configs") {

        //    var filePath = Path.Combine(AppContext.BaseDirectory, baseDir);
        //    JObject jsonObject;
        //    using (StreamReader file = new StreamReader(filePath))
        //    using (JsonTextReader reader = new JsonTextReader(file))
        //    {
        //        jsonObject = (JObject)JToken.ReadFrom(reader);
        //        jsonObject["appSettings"]["Key"] = "asdasdasdasd";
        //    }

        //    using (var writer = new StreamWriter(filePath))
        //    using (JsonTextWriter jsonwriter = new JsonTextWriter(writer))
        //    {
        //        jsonObject.WriteTo(jsonwriter);
        //    }
        //    return "";
        //}
    }
}
