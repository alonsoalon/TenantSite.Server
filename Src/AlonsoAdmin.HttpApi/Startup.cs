using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.Upload;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.HttpApi.Extensions;
using AlonsoAdmin.HttpApi.Filters;
using AlonsoAdmin.HttpApi.Logs;
using AlonsoAdmin.MultiTenant;
using AlonsoAdmin.MultiTenant.Extensions;
using AlonsoAdmin.Repository;
using AlonsoAdmin.Tasks;
using FreeSql;
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
using Quartz.Spi;
using System.Collections.Generic;

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

            //�õ�������Ч��������
            //_startupConfig = SettingHelper.Get<StartupConfig>("startupsettings", env.EnvironmentName) ?? new StartupConfig();
        }

        // ����ʱ���ô˷�����ʹ�ô˷�����������ӷ���
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

            // �õ�����������Ҫ�Ĳ�������        
            var _startupConfig = Configuration.GetSection("Startup").Get<StartupConfig>();

            // ע��ϵͳ���õ�����
            services.Configure<SystemConfig>(Configuration.GetSection("System"));

            // ע���⻧���õ�����
            services.Configure<List<TenantInfo>>(Configuration.GetSection("Tenants"));


            services.AddControllers(options =>
            {
                if (_startupConfig.Log.Operation)
                {
                    options.Filters.Add<LogActionFilter>();                    
                }
                options.Filters.Add<ValidateModelFilter>();// �Զ��� ģ����֤
            })
            .ConfigureApiBehaviorOptions(options => {
                //�ر�Ĭ��ģ����֤,��Ϊ����ʹ�����Լ���ValidateModelFilter
                options.SuppressModelStateInvalidFilter = true;
            })
            // �趨json���л�����
            .AddNewtonsoftJson(options =>
            {
                // ����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // ʹ��С�շ�
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // new DefaultContractResolver();
                // ����ʱ���ʽ
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
                    // ÿ���⻧�����в�ͬ�����ò����������⻧�������ò��� (JwtBearerOptions:o)���ԣ�
                    // ��RegsiterMultiTenantServices WithPerTenantOptions<JwtBearerOptions>
                });

            //ע���ϴ�������
            services.AddSingleton<IUploadTool, UploadTool>();

            #region ����
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

           

            // ע�� AutoMapper 
            services.RegisterMapper();
            // ע�� ���⻧ ����
            services.RegsiterMultiTenantServices(_startupConfig.TenantRouteStrategy);
            // ע�� ������Ŀ�Ͳִ���Ŀ 
            services.RegsiterServicesAndRepositories(Env);
            // ע��Ȩ�����
            services.RegsiterPermissionServices(Env);
            // ע��Swagger
            services.RegisterSwagger();

            // ע����־�������Ҫ���ڷ�����Ŀע�����֮����Ϊ����������־����
            if (_startupConfig.Log.Operation)
            {
                services.AddScoped<ILogHandler, LogHandler>();
            }

            #region ע��ƻ�����
            services.AddJobSetup();
            #endregion

        }

        // ����ʱ���ô˷�����ʹ�ô˷�������HTTP����ܵ���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ����swagger�м��
            app.UseSwaggerMiddleware();

            // ȫ���쳣����
            app.UseErrorHandlingMiddleware();

            //��̬�ļ�
            app.UseUploadConfig();

            // ·���м��
            app.UseRouting();

            // ������
            app.UseCors(_allowSpecificOrigins);

            // ���ö��⻧�м��
            app.UseMultiTenant();

            // ����Authentication�м�������������е������֤������ȡ����֤�������ϲ�����HttpContext.User��
            app.UseAuthentication();

            // ���������Ȩ����֤
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
