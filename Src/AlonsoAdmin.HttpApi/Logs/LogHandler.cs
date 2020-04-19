using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            var req = new OprationLogRequest
            {
                ApiMethod = context.HttpContext.Request.Method.ToLower(),
                ApiPath = GetCurrentRoutePath(),
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res?.Success,
                Message = res?.Message,

                Browser = client.UA.Family,
                Os = client.OS.Family,
                Device = device.ToLower() == "other" ? "" : device,
                BrowserInfo = ua,
                Ip = IPHelper.GetIP(_accessor?.HttpContext?.Request),
                RealName = _authUser.DisplayName,
                Params = args,
                Result = result
            };
            
            await _oprationLogService.AddAsync(req);
        }


        private string GetCurrentRoutePath()
        {
            //var api= context.ActionDescriptor.AttributeRouteInfo.Template;
            //var questUrl = _accessor.HttpContext.Request.Path.Value.ToLower();
            var routeValues = _accessor.HttpContext.Request.RouteValues;            
            routeValues.TryGetValue("controller", out object controller);
            routeValues.TryGetValue("action", out object action);
            string currentRoutePath = controller.ToString() + "/" + action.ToString();
            return currentRoutePath;
        }
    }
}
