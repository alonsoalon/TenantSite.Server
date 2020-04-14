using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.HttpApi.Auth;
using AlonsoAdmin.HttpApi.AuthStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi
{
    public static class PermissionExtensions
    {

        /// <summary>
        /// 注册权限验证服务相关
        /// </summary>
        /// <param name="services"></param>
        public static void RegsiterPermissionServices(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.TryAddSingleton<IAuthUser, AuthUser>();
            services.TryAddSingleton<IAuthToken, AuthToken>();


            var permissionRequirement = new PermissionRequirement();
            services.AddTransient<UserStore>(); // 注册用户与角色关系数据类，PermissionHandler需要用到
            services.AddTransient<ApiStore>();
            //services.AddSingleton(x => { return new ApiStore(x.GetRequiredService<ISysApiService>()); });  // 注册API与角色关系数据类，PermissionHandler需要用到
            services.AddTransient<IAuthorizationHandler, PermissionHandler>();
    
            // 注册权限验证策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("default",
                         policy => policy.Requirements.Add(permissionRequirement));
            });


        }
    }
}
