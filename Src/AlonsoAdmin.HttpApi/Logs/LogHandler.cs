using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Entities;
using AlonsoAdmin.HttpApi.SwaggerHelper;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Logs
{
    public class LogHandler : ILogHandler
    {
        private readonly ISysOprationLogService _oprationLogService;
        private readonly IAuthUser _authUser;
        private readonly IHttpContextAccessor _accessor;

        
        public LogHandler(
            ISysOprationLogService oprationLogService,
            IAuthUser authUser,
            IHttpContextAccessor accessor
        )
        {
            _oprationLogService = oprationLogService;
            _authUser = authUser;
            _accessor = accessor;
        }

        public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();

            dynamic actionResult = (await next()).Result;

            sw.Stop();

            //操作参数
            var args = JsonConvert.SerializeObject(context.ActionArguments,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            //操作结果
            var result = JsonConvert.SerializeObject(actionResult?.Value,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var res = actionResult?.Value as IResponseEntity;

            string ua = _accessor.HttpContext.Request.Headers["User-Agent"];
            var client = UAParser.Parser.GetDefault().Parse(ua);
            var device = client.Device.Family;

            var req = new OprationLogAddRequest
            {
                ApiTitle = GetCurrentActionDesc(context),
                ApiPath = GetCurrentRoutePath(context),
                ApiMethod = context.HttpContext.Request.Method.ToLower(),               
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Message = res.Message,

                Browser = client.UA.Family,
                Os = client.OS.Family,
                Device = device.ToLower() == "other" ? "" : device,
                BrowserInfo = ua,
                Ip = IPHelper.GetIP(_accessor?.HttpContext?.Request),
                RealName = _authUser.DisplayName,
                Params = args,
                Result = result
            };
            
            await _oprationLogService.CreateAsync(req);
        }


        private string GetCurrentRoutePath(ActionExecutingContext context)
        {
            var routeValues = _accessor.HttpContext.Request.RouteValues;
            routeValues.TryGetValue("controller", out object controller);
            routeValues.TryGetValue("action", out object action);

            var moduleBaseControllerType = context.Controller.GetType().BaseType;
            string pathTemplate = string.Empty;
            if (moduleBaseControllerType.GetCustomAttributes(typeof(CustomRouteAttribute)).Any())
            {
                var routeAttr = (moduleBaseControllerType.GetCustomAttribute(typeof(CustomRouteAttribute)) as CustomRouteAttribute);
                // 加.Replace("{__tenant__}/", "")为了兼容在多租户非Host模式下的问题
                pathTemplate = routeAttr.Template.Replace("{__tenant__}/", "");

            }

            return pathTemplate.Replace("[controller]", controller.ToString()).Replace("[action]", action.ToString());
        }

        private string GetCurrentActionDesc(ActionExecutingContext context) {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //var controllerName = actionDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;

            string desc = string.Empty;
            var methodInfo = actionDescriptor.MethodInfo;
            if (methodInfo.GetCustomAttributes(typeof(DescriptionAttribute)).Any())
            {
                desc = (methodInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description;
            }

            // 处理固定接口描述
            if (desc == string.Empty)
            {
                switch (actionName)
                {
                    case "Create":
                        desc = "创建记录";
                        break;
                    case "Update":
                        desc = "更新记录";
                        break;
                    case "Delete":
                        desc = "删除记录-物理删除单条";
                        break;
                    case "DeleteBatch":
                        desc = "删除记录-物理删除批量";
                        break;
                    case "SoftDelete":
                        desc = "删除记录-软删除单条";
                        break;
                    case "SoftDeleteBatch":
                        desc = "删除记录-软删除批量";
                        break;
                    case "GetItem":
                        desc = "得到实体-根据主键ID";
                        break;
                    case "GetList":
                        desc = "取得条件下数据(分页)";
                        break;
                    case "GetAll":
                        desc = "取得条件下数据(不分页)";
                        break;
                }
            }

            return desc;

        }
    }
}
