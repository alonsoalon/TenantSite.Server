using AlonsoAdmin.Entities;
using AlonsoAdmin.HttpApi.AuthStore;
using AlonsoAdmin.Services.System;
using AlonsoAdmin.Services.System.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Auth
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ApiStore _apiStore;
        private readonly UserStore _userStore;

        private readonly ISysUserService _sysUserService;

        public PermissionHandler(IHttpContextAccessor accessor, UserStore userStore, ApiStore apiStore, ISysUserService sysUserService)
        {



            this._accessor = accessor;
            this._apiStore = apiStore;
            this._userStore = userStore;

            _sysUserService = sysUserService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var response = _accessor.HttpContext.Response;

            var currentUser = context.User;

            if (!currentUser.Identity.IsAuthenticated)  // 未登陆
            {

                ////var payload = JsonConvert.SerializeObject(ResponseEntity.Error("未登录，您无权访问该接口", "401"));

                //var payload = JsonConvert.SerializeObject(
                //   ResponseEntity.Error("未登录，您无权访问该接口", "401"),
                //   Formatting.None,
                //   new JsonSerializerSettings
                //   {
                //       ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                //   });


                //// 定义返回的数据类型
                //response.ContentType = "application/json";
                //// 定义返回状态
                //response.StatusCode = StatusCodes.Status401Unautorized;
                ////输出Json数据结果
                //await response.WriteAsync(payload);

                throw new Exception("未登录，您无权访问该接口");
                //context.Fail();
                //return;
            }
            

            //通过请求路由表得到请求的API
            var currentApi = GetCurrentRoutePath().ToLower();
            // 取得当前访问的API对象
            var storeApi = _apiStore.Find(currentApi);

            //var api= _sysApiService.FindByUrl(currentApi);

            //var a = _sysUserService.GetCurrentUserRolesAsync();

            if (storeApi == null)
            {
                // 方案1：未注册的API，需授权才可访问
                // await ResponseErrAsync(response, "401", "当前API未配置到指定功能，无权访问!");
                // context.Fail();

                // 方案2：未注册的API，视为不用授权即可访问
                context.Succeed(requirement);
                return;
            }

            if (storeApi.Roles == null || !storeApi.Roles.Any())
            {
                // 需对api进行授权
                //await ResponseErrAsync(response, "401", "当前API未配置到任何角色，无权访问!");
                //{ Code = "401", Message = "当前API未配置到任何角色，无权访问!" }

                var payload = JsonConvert.SerializeObject(ResponseEntity.Error("当前API未配置到任何角色，无权访问","401"));
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status401Unauthorized;
                await response.WriteAsync(payload);

                return;
            }

            // requirement.AllowedRoles = storeApi.Roles;

            // 得到当前用户ID
            var currentUserId = currentUser.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var storeUser = _userStore.Find(currentUserId);
            if (storeUser == null) // 在存储中未找到用户
            {
                var payload = JsonConvert.SerializeObject(ResponseEntity.Error("用户信息错误，无权访问", "401"));
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status401Unauthorized;
                await response.WriteAsync(payload);
                return;
            }

            if (storeUser.Roles == null || !storeUser.Roles.Any()) // 用户未配置任何角色
            {
                var payload = JsonConvert.SerializeObject(ResponseEntity.Error("用户未配置任何角色，无权访问", "401"));
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status401Unauthorized;
                await response.WriteAsync(payload);
                return;
            }

            // 验证用户角色与API角色是否有交集
            bool found = storeUser.Roles.Any(r => storeApi.Roles.Contains(r));  //storeApi.Roles.Any(r => storeUser.Roles.Contains(r));

            if (found)
            {

                context.Succeed(requirement);

            }
            else
            {
                var payload = JsonConvert.SerializeObject(ResponseEntity.Error("很抱歉，您无权访问该接口", "401"));
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status401Unauthorized;
                await response.WriteAsync(payload);
                return;
            }
        }

        /// <summary>
        /// 得到当前访问路由的path
        /// </summary>
        /// <returns></returns>
        private string GetCurrentRoutePath()
        {

            var routeValues = _accessor.HttpContext.Request.RouteValues;
            //var questUrl = _httpContextAccessor.HttpContext.Request.Path.Value.ToLower();

            routeValues.TryGetValue("controller", out object controller);
            routeValues.TryGetValue("action", out object action);
            string currentRoutePath = controller.ToString() + "/" + action.ToString();
            return currentRoutePath;
        }




    }
}
