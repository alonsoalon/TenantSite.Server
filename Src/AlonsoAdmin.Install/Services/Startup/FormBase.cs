using AlonsoAdmin.Common.Configs;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services.Startup
{
    public class FormBase: ComponentBase
    {

        [Inject]
        ISettingService SettingService { get; set; }
      
        [Inject]
        MessageBox MessageBox { get; set; }

        internal object value;

        protected BForm configForm;


        protected void Submit()
        {
            if (!configForm.IsValid())
            {
                return;
            }

            try
            {
                var entity = configForm.GetValue<StartupEntity>();
                var result = SettingService.WriteConfig<StartupConfig>("Startup", opt =>
                   {
                       opt.Cache.Type = entity.CacheType;
                       opt.Cache.Redis.ConnectionString = entity.RedisConnectionString;
                       opt.Log.Operation = entity.OperationlogOpen;
                       opt.TenantRouteStrategy = entity.TenantRouteStrategy;
                   });
                _ = MessageBox.AlertAsync("更新成功");
            }
            catch (Exception ex) {
                _ = MessageBox.AlertAsync(ex.Message);
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            var item = SettingService.GetStartupSettingsAsync().Result;

            value = new StartupEntity()
            {
                CacheType = item.Cache.Type,
                RedisConnectionString = item.Cache.Redis.ConnectionString,
                OperationlogOpen = item.Log.Operation,
                TenantRouteStrategy = item.TenantRouteStrategy
            };
        }

        protected void Reset()
        {
            configForm.Reset();
        }

    }
}
