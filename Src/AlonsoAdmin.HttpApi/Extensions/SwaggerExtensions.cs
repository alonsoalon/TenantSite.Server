using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi
{
    public static class SwaggerExtensions
    {
        
        public static void RegisterSwagger(this IServiceCollection services)
        {

            string ApiName = "AlonsoAdmin";

            #region Swagger 接口文档定义
            // 注册Swagger生成器，定义一个或多个Swagger文档
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                //根据版本名称倒序 遍历展示
                typeof(ApiVersions).GetEnumNames().OrderBy(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} API",
                        Description = "MultiTenant + MultiDatabase. SingleTenant + MultiDatabase. MultiApiVersion. Base on Netcore3.x + FreeSql",
                        TermsOfService = new Uri("https://www.xxx.com/"),//服务条款
                        Contact = new OpenApiContact
                        {
                            Name = "Alonso",
                            Email = string.Empty
                            //Url = new Uri("https://www.xxx.com/alonso"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "License",
                            Url = new Uri("https://www.xxx.com/license"),
                        }
                    });

                });




                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    //添加注释到SwaggerUI
                    c.IncludeXmlComments(xmlPath);
                }

                #region 为SwaggerUI添加全局token验证
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{ Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme,Id = "Bearer" }},
                        new string[] { }
                    }});
                #endregion
            });
            #endregion

        }

        public static void UseSwaggerMiddleware(this IApplicationBuilder app) {

            string ApiName = "AlonsoAdmin";

            //启用swagger中间件
            app.UseSwagger();

            //启用中间件服务swagger-ui (HTML, JS, CSS等)，
            //指定Swagger JSON端点。
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //根据版本名称倒序 遍历展示
                typeof(ApiVersions).GetEnumNames().OrderBy(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });

            });

        }
    }
}
