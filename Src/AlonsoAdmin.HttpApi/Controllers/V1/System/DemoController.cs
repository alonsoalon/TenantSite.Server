using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.HttpApi.SwaggerHelper;
using AlonsoAdmin.MultiTenant.Extensions;
using AlonsoAdmin.Services;
using AlonsoAdmin.Services.System;
using AlonsoAdmin.Services.System.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{


    [Description("示例")]
    public class DemoController : ModuleBaseController
    {

        private ILogger<DemoController> Logger { get; }
        private ISysApiService SysApiService { get; }

        public DemoController(ILogger<DemoController> logger,  ISysApiService _SysApiService)
        {
            SysApiService = _SysApiService;
            Logger = logger;
        }

        
        [HttpGet]
        public IResponseEntity Get()
        {
            var ti = HttpContext.GetMultiTenantContext()?.TenantInfo;
            Logger.LogInformation($"{typeof(DemoController)}.Get 执行完成");
            return ResponseEntity.Ok(ti);
        }

        

    }
}
