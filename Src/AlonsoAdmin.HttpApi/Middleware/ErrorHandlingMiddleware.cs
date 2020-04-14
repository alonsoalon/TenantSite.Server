using AlonsoAdmin.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this._logger = logger;
        }
        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

        }


        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var code = StatusCodes.Status500InternalServerError;

            string info;
            switch (context.Response.StatusCode)
            {
                case 401:
                    info = "没有权限";
                    break;
                case 404:
                    info = "未找到服务";
                    break;
                case 403:
                    info = "服务器理解请求客户端的请求，但是拒绝执行此请求";
                    break;
                case 500:
                    info = "服务器内部错误，无法完成请求";
                    break;
                case 502:
                    info = "请求错误";
                    break;
                default:
                    info = ex.Message;
                    break;
            }

            _logger.LogError(info); // todo:可记录日志,如通过注入Nlog等三方日志组件记录

            //var result = JsonConvert.SerializeObject(new { Coede= code.ToString(), Message = info });
            var result = JsonConvert.SerializeObject(
                ResponseEntity.Error(info, code),
                Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(result);
        }
    }
}
