using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.HttpApi.Filters;
using AlonsoAdmin.HttpApi.Logs;
using AlonsoAdmin.MultiTenant.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AlonsoAdmin.HttpApi
{
    public class Startup
    {
 
        private IWebHostEnvironment Env { get; }

        public IConfiguration Configuration { get; }

        //private readonly StartupConfig _startupConfig;

        readonly string _allowSpecificOrigins = "_allowSpecificOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;

            //得到启动生效的配置项
            //_startupConfig = SettingHelper.Get<StartupConfig>("startupsettings", env.EnvironmentName) ?? new StartupConfig();
        }

        // 运行时调用此方法。使用此方法向容器添加服务。
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_allowSpecificOrigins, builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                //.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
                );
                //.AllowAnyMethod()
                //.AllowAnyHeader()
                //.AllowCredentials());
            });


            // 注册启动配置到容器
            services.Configure<SystemConfig>(Configuration.GetSection("System"));

            // 得到程序启动需要的参数配置        
            var _startupConfig = Configuration.GetSection("Startup").Get<StartupConfig>();

            // 注册系统配置到容器
            services.Configure<SystemConfig>(Configuration.GetSection("System"));

            services.AddControllers(options =>
            {
                if (_startupConfig.Log.Operation)
                {
                    options.Filters.Add<LogActionFilter>();
                }
            })
            // 设定json序列化参数
            .AddNewtonsoftJson(options =>
            {
                // 忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 使用小驼峰
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // new DefaultContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";             


            });


            services
                .AddAuthentication(x =>
                {
                      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                      x.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme; 
                })
                //.AddCookie(options =>
                //{
                //    options.SlidingExpiration = true;
                //})
                .AddJwtBearer(o =>
                {
                    // 每个租户可以有不同的配置参数，故在租户里面配置参数 (JwtBearerOptions:o)属性，
                    // 见RegsiterMultiTenantServices WithPerTenantOptions<JwtBearerOptions>
                });


            #region 缓存
            var cacheSettings = _startupConfig.Cache;
            if (cacheSettings.Type == CacheType.Redis)
            {
                var csredis = new CSRedis.CSRedisClient(cacheSettings.Redis.ConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            #endregion



            



            // 注册 AutoMapper 
            services.RegisterMapper();
            // 注册 多租户 服务
            services.RegsiterMultiTenantServices(_startupConfig.TenantRouteStrategy);
            // 注册 服务项目和仓储项目 
            services.RegsiterServicesAndRepositories(Env);
            // 注册权限相关
            services.RegsiterPermissionServices(Env);
            // 注册Swagger
            services.RegisterSwagger();

            // 注册日志处理对象，要放在服务项目注册完成之后，因为依赖操作日志服务
            if (_startupConfig.Log.Operation)
            {
                services.AddScoped<ILogHandler, LogHandler>();
            }


           

        }

        // 运行时调用此方法。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 启用swagger中间件
            app.UseSwaggerMiddleware();

            // 全局异常捕获
            app.UseErrorHandlingMiddleware();

            // 路由中间件
            app.UseRouting();

            app.UseCors(_allowSpecificOrigins);


            // 启用多租户中间件
            app.UseMultiTenant();

            // 启用Authentication中间件，遍历策略中的身份验证方案获取多张证件，最后合并放入HttpContext.User中
            app.UseAuthentication();
            // 对请求进行权限验证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute("default", "{__tenant__=tenant1}/api/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute("default", "{__tenant__=}/api/{controller=Home}/{action=Index}");
            });
        }
    }
}
