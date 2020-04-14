using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.MultiTenant.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi
{
    public static class MultiTenantExtensions
    {
        /// <summary>
        /// 注册多租户
        /// </summary>
        /// <param name="services"></param>
        /// <param name="routeStrategy">路由策略</param>
        public static void RegsiterMultiTenantServices(this IServiceCollection services, TenantRouteStrategy routeStrategy) {


            // 目前采用读取配置文件来获取租户信息，计划将来通过配置读取模式来切换 1.配置文件读取 2.直连数据库 3.远程接口

            #region 多租户
            var ser = services.AddMultiTenant()
                .WithConfigurationStore();

            switch (routeStrategy) {
                case TenantRouteStrategy.Route:
                    ser = ser.WithRouteStrategy(); // 针对形如: www.abc.com/tenant/的解析
                    break;
                case TenantRouteStrategy.Host:
                    ser = ser.WithHostStrategy(); // 二级域名策略 针对形如:tenant.abc.com的解析
                    break;
                default:
                     ser = ser.WithRouteStrategy();
                    break;
            }
            ser.WithPerTenantOptions<JwtBearerOptions>((o, tenantInfo) =>
                {
                    //o.RequireHttpsMetadata = false;                    
                    //o.Challenge = JwtBearerDefaults.AuthenticationScheme;
                    //o.Authority = (string)tenantInfo.Items["Authority"];
                    //o.Audience = (string)tenantInfo.Items["Audience"];

                    //使用应用密钥得到一个加密密钥字节数组
                    var key = Encoding.ASCII.GetBytes((string)tenantInfo.Items["Secret"]);

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = (string)tenantInfo.Items["Issuer"],

                        ValidateAudience = true,
                        ValidAudience = (string)tenantInfo.Items["Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,//是否验证超时  当设置exp和nbf同时有效 同时启用ClockSkew                         
                        ClockSkew = TimeSpan.FromSeconds(30) //这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟

                    };
                });
            #endregion
        }
    }
}
