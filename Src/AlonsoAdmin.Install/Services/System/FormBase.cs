using AlonsoAdmin.Common.Configs;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services.System
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
                var entity = configForm.GetValue<SystemConfig>();
                var result = SettingService.WriteConfig<SystemConfig>("System", opt =>
                   {
                       opt = entity;
                   });
                _ = MessageBox.AlertAsync("更新成功");
            }
            catch (Exception ex)
            {
                _ = MessageBox.AlertAsync(ex.Message);
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            var item = SettingService.GetSystemSettingsAsync().Result;
            value = item;
        }

        protected void Reset()
        {
            configForm.Reset();
        }

    }
}
