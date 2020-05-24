using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.HttpApi.Exceptions;
using AlonsoAdmin.MultiTenant.Extensions;
using AlonsoAdmin.Services.System.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Auth
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IOptionsMonitor<StartupConfig> _startupConfig;
        private readonly IOptionsMonitor<SystemConfig> _systemConfig;
        private readonly IAuthService _authService;

        public PermissionHandler(
            IHttpContextAccessor accessor,
            IOptionsMonitor<StartupConfig> startupConfig,
            IOptionsMonitor<SystemConfig> systemConfig,
            IAuthService authService)
        {



            this._accessor = accessor;
            this._startupConfig = startupConfig;
            this._systemConfig = systemConfig;
            this._authService = authService;

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {           

            var currentUser = context.User;

            if (!currentUser.Identity.IsAuthenticated)  
            {
                throw new UnLoginException("未登录!");
            }

            if (_systemConfig.CurrentValue.EnableApiAccessControl)
            {
                var currentApi = GetCurrentApi();
                var isAccess = await _authService.VerifyUserAccessApiAsync(currentApi);

                if (isAccess)
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    throw new UnAuthorizeException("您无权访问接口：" + currentApi + ", 需授权");
                }
            }

            context.Succeed(requirement);
        }

        /// <summary>
        /// 得到当前访问路由的path
        /// </summary>
        /// <returns></returns>
        private string GetCurrentApi()
        {
            string path = _accessor.HttpContext.Request.Path;
            if (_startupConfig.CurrentValue.TenantRouteStrategy == TenantRouteStrategy.Route) {
                var tenantInfo = _accessor.HttpContext.GetMultiTenantContext()?.TenantInfo;
                path = path.ToLower().Replace("/" + tenantInfo.Code.ToLower() + "/", "/");
            }

            return path;
        }


    }
}
