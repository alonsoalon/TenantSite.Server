using AlonsoAdmin.HttpApi.SwaggerHelper;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi.Controllers.V1.Dictionary
{
    /// <summary>
    /// 模块控制器基类，用于统一模块的路由地址（版本+模块名称）
    /// 继承 BaseController，在BaseController已定义了权限策略
    /// </summary>
    [CustomRoute(ApiVersions.v1, "Dictionary")]
    public abstract class ModuleBaseController : BaseController
    {    
    }
}