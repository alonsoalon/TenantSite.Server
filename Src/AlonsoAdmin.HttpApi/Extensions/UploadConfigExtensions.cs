using AlonsoAdmin.Common.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Extensions
{
    public static class UploadConfigExtensions
    {
        private static void UseFileUploadConfig(IApplicationBuilder app, FileUploadConfig config)
        {
            if (!Directory.Exists(config.UploadPath))
            {
                Directory.CreateDirectory(config.UploadPath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = config.RequestPath,
                FileProvider = new PhysicalFileProvider(config.UploadPath)
            });
        }

        public static void UseUploadConfig(this IApplicationBuilder app)
        {
            var systemConfig = app.ApplicationServices.GetRequiredService<IOptions<SystemConfig>>();
            UseFileUploadConfig(app, systemConfig.Value.UploadAvatar);
        }
    }
}
