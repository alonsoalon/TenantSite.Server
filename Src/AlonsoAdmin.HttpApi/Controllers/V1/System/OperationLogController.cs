using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{
    [Description("操作日志")]
    public class OperationLogController : ModuleBaseController
    {
        private readonly ISysOprationLogService _sysOprationLogService;

        public OperationLogController(ISysOprationLogService sysOprationLogService)
        {
            _sysOprationLogService = sysOprationLogService;
        }

        /// <summary>
        /// 查询分页操作日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<OprationLogFilterRequest> req)
        {
            return await _sysOprationLogService.GetListAsync(req);
        }
    }
}