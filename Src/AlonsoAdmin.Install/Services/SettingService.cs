using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Install.Utils;
using AlonsoAdmin.MultiTenant;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services
{
    public class SettingService : ISettingService
    {

        private IWebHostEnvironment Env { get; }

        public SettingService(IWebHostEnvironment env)
        {
            Env = env;
        }


        private string ConfigDir {
            get {

                var baseDirectory = AppContext.BaseDirectory;
                var filePath = baseDirectory.Substring(0, baseDirectory.IndexOf("AlonsoAdmin.Install")) + "AlonsoAdmin.HttpApi";
                return filePath;
            }
        }

        public Task<SystemConfig> GetSystemSettingsAsync()
        {
            var apiAppSettings = SettingHelper.Load("appsettings", ConfigDir, Env.EnvironmentName);
            var settings = apiAppSettings.GetSection("System").Get<SystemConfig>();
            return Task.FromResult(settings);
        }

        public Task<StartupConfig> GetStartupSettingsAsync()
        {

            var apiAppSettings = SettingHelper.Load("appsettings", ConfigDir, Env.EnvironmentName);
            var settings = apiAppSettings.GetSection("Startup").Get<StartupConfig>();
            return Task.FromResult(settings);
        }

        public Task<List<MultiTenant.TenantInfo>> GetTenantListAsync()
        {
            var apiAppSettings = SettingHelper.Load("appsettings", ConfigDir, Env.EnvironmentName);
            var settings = apiAppSettings.GetSection("Tenants").Get<List<MultiTenant.TenantInfo>>();
            return Task.FromResult(settings);
        }


        public bool WriteConfig<T>(string node, Action<T> applyChanges) 
        {

            var filePath = Path.Combine(ConfigDir, "appsettings.json");

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));            
            if (jObject.TryGetValue(node, out JToken section))
            {
                var sectionObject = JsonConvert.DeserializeObject<T>(section.ToString());

                applyChanges(sectionObject);

               
                jObject[node] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            
       
                
                File.WriteAllText(filePath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WriteTenantsConfig(List<TenantInfo> tenants)
        {
            string node = "Tenants";
            var filePath = Path.Combine(ConfigDir, "appsettings.json");

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));
            if (jObject.TryGetValue(node, out JToken section))
            {
                //var sectionObject = JsonConvert.DeserializeObject<T>(section.ToString());

                //applyChanges(sectionObject);

                jObject[node] = JArray.Parse(JsonConvert.SerializeObject(tenants));
                

                File.WriteAllText(filePath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
