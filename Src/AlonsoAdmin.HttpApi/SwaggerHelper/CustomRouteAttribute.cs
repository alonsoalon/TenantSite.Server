
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi.SwaggerHelper
{
    /// <summary>
    /// 自定义路由 /api/{version}/[controler]/[action]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {

        /// <summary>
        /// 分组名称,是来实现接口 IApiDescriptionGroupNameProvider
        /// </summary>
        public string GroupName { get; set; }
        //[Route("{__tenant__=tenant1}/api/[controller]/[action]")]
        /// <summary>
        /// 自定义路由构造函数，继承基类路由
        /// </summary>
        /// <param name="actionName"></param>
        public CustomRouteAttribute(string actionName = "") : base(actionName)
        {
        }
        /// <summary>
        /// 自定义版本+路由构造函数，继承基类路由
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="version"></param>
        public CustomRouteAttribute(ApiVersions version, string actionName = "[action]") : base(GetRouteTemplate(version, actionName))
        {
            GroupName = version.ToString();
        }

        private static string GetRouteTemplate(ApiVersions version, string actionName = "[action]")
        {
            string temp= "/{__tenant__}/api/" + version.ToString() + "/[controller]/" + actionName;
            var startupConfig = StartupConfigHelper.Get();
            switch (startupConfig.TenantRouteStrategy) {

                case TenantRouteStrategy.Route:                      
                    temp = "/{__tenant__}/api/" + version.ToString() + "/[controller]/" + actionName;
                    break;
                case TenantRouteStrategy.Host:
                    temp = "/api/" + version.ToString() + "/[controller]/" + actionName;  
                    break;
            }
            return temp;
        }
    }

   
}
