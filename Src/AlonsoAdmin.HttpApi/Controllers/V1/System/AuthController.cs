
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.Entities;
using AlonsoAdmin.HttpApi.Attributes;
using AlonsoAdmin.HttpApi.Auth;
using AlonsoAdmin.HttpApi.SwaggerHelper;
using AlonsoAdmin.MultiTenant.Extensions;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{
   
    public class AuthController : ModuleBaseController
    {
        private IAuthToken _authToken;
        private readonly IAuthService _authService;

        private readonly ISysLoginLogService _loginLogService;
        public AuthController(
            IAuthToken authToken, 
            IAuthService authServices,
            ISysLoginLogService loginLogService
            )
        {
            _authToken = authToken;
            _authService = authServices;
            _loginLogService = loginLogService;
        }
       
        
        [AllowAnonymous]
        [HttpPost]
        [NoOprationLog]
        public async Task<IResponseEntity> Login(AuthLoginRequest req) {

            var sw = new Stopwatch();
            sw.Start();
            var res = (await _authService.LoginAsync(req)) as IResponseEntity;
            sw.Stop();

            if (!res.Success)
            {
                return res;
            }
            else
            {

                var user = (res as IResponseEntity<AuthLoginResponse>).Data;


                #region 写登录日志

                string ua = HttpContext.Request.Headers["User-Agent"];
                var client = UAParser.Parser.GetDefault().Parse(ua);
                var device = client.Device.Family;
                var loginLogAddRequest = new LoginLogAddRequest()
                {
                    CreatedBy = user.Id,
                    CreatedByName = user.UserName,
                    RealName = user.DisplayName,
                    ElapsedMilliseconds = sw.ElapsedMilliseconds,
                    Status = res.Success,
                    Message = res.Message,

                    Browser = client.UA.Family,
                    Os = client.OS.Family,
                    Device = device.ToLower() == "other" ? "" : device,
                    BrowserInfo = ua,
                    Ip = IPHelper.GetIP(HttpContext?.Request)
                };

                await _loginLogService.AddAsync(loginLogAddRequest);
                #endregion

                #region 构造JWT Token
                var claims = new Claim[]{
                    new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                    new Claim(ClaimAttributes.UserName, user.UserName),
                    new Claim(ClaimAttributes.DisplayName,user.DisplayName)
                };
                var token = _authToken.Build(claims);
                #endregion

                var data = new
                {
                    token,
                    uuid = user.Id,
                    info = new
                    {
                        name = user.UserName,
                        displayNamme = user.DisplayName,
                        avatar = user.Avatar
                    }
                };

                return ResponseEntity.Ok(data);
            }
        }

        [HttpGet]
        public async Task<IResponseEntity> UserInfo()
        {

            var res = await _authService.GetUserInfoAsync();
            var user = (res as IResponseEntity<AuthLoginResponse>).Data;
            if (!res.Success)
            {
                return res;
            }

            #region 构造JWT Token
            var claims = new Claim[]{
                    new Claim(ClaimAttributes.UserId, user.Id.ToString()),
                    new Claim(ClaimAttributes.UserName, user.UserName),
                    new Claim(ClaimAttributes.DisplayName,user.DisplayName)
                };
            var token = _authToken.Build(claims);
            #endregion


            var data = new
            {
                uuid = user.Id,
                token,
                info = new
                {
                    name = user.UserName,
                    displayNamme = user.DisplayName,
                    avatar = user.Avatar
                },
                menus = user.Menus
            };

            return ResponseEntity.Ok(data);
        }
    }
}