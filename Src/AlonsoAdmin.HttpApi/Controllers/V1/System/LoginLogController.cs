using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{

    [Description("登录日志")]
    public class LoginLogController : ModuleBaseController
    {
        private readonly ISysLoginLogService _sysLoginLogService;

        public LoginLogController(ISysLoginLogService sysLoginLogService)
        {
            _sysLoginLogService = sysLoginLogService;
        }

        /// <summary>
        /// 查询分页登录日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<LoginLogFilterRequest> req)
        {
            return await _sysLoginLogService.GetListAsync(req);
        }
    }
}