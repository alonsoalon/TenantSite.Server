using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Controllers
{
    /// <summary>
    /// 控制器基础类，存在目的主要是配置Route，解决目前没找到Swagger如何在Startup.cs中获取路由的的方法，故通过基础控制器类配置ROUTE来处理
    /// 如果不用Swagger,路由可统一在 app.UseEndpoints中配置：endpoints.MapControllerRoute("default", "{__tenant__=tenant1}/api/{controller=Home}/{action=Index}");
    /// </summary>
    //[Route("{__tenant__=tenant1}/api/[controller]/[action]")]
    [ApiController]
    [Authorize("default")]
    public class BaseController : ControllerBase
    {

    }
 
}
