using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.HttpApi.SwaggerHelper;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{
    [Description("接口管理")]
    public class ApiController : ModuleBaseController
    {
        private readonly ISysApiService _sysApiService;
        public ApiController(ISysApiService sysApiService)
        {
            _sysApiService = sysApiService;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(ApiAddRequest req)
        {
            return await _sysApiService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(ApiEditRequest req)
        {
            return await _sysApiService.UpdateAsync(req);
        }

        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _sysApiService.DeleteAsync(id);
        }

        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _sysApiService.DeleteBatchAsync(ids);
        }

        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _sysApiService.SoftDeleteAsync(id);
        }



        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _sysApiService.SoftDeleteBatchAsync(ids);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _sysApiService.GetItemAsync(id);
        }

        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<ApiFilterRequest> req)
        {
            return await _sysApiService.GetListAsync(req);
        }

        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(ApiFilterRequest req)
        {
            return await _sysApiService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法
        /// <summary>
        ///  自动生成API,当API存在时更新
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("自动生成API,当API存在时更新")]
        public async Task<IResponseEntity> GenerateApis()
        {
            //接口列表
            List<SysApiEntity> list = new List<SysApiEntity>();

            // 反射取回所有控制器
            var dllTypes = Assembly.GetExecutingAssembly()
                .GetTypes().Where(t => t.FullName.EndsWith("Controller"));

            // 取得API接口控制器（排除基类控制器）
            var controllers = dllTypes
                .Where(t => !t.FullName.EndsWith("ModuleBaseController")&& !t.FullName.EndsWith("BaseController"));
           

            foreach (var controller in controllers)
            {
                var members = controller.GetMethods()
                     .Where(m => typeof(Task<IResponseEntity>).IsAssignableFrom(m.ReturnType));

                //验证模块基类
                var moduleBaseController = dllTypes.Where(t => t == controller.BaseType).FirstOrDefault();
                if (moduleBaseController == null)
                {
                    continue;
                }

                //获取API路由模板
                string pathTemplate = string.Empty;
                if (moduleBaseController.GetCustomAttributes(typeof(CustomRouteAttribute)).Any())
                {
                    var routeAttr = (moduleBaseController.GetCustomAttribute(typeof(CustomRouteAttribute)) as CustomRouteAttribute);
                    pathTemplate = routeAttr.Template;
                }
                else
                {
                    continue;
                }

                //获取controller描述
                string controllerDesc = string.Empty;
                if (controller.GetCustomAttributes(typeof(DescriptionAttribute)).Any())
                {
                    controllerDesc = (controller.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute).Description;
                }


                foreach (var member in members)
                {
                    // 获取API描述
                    object[] attrs = member.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    string desc = string.Empty;
                    if (attrs.Length > 0)
                        desc = (attrs[0] as DescriptionAttribute).Description;

                    //获取参数
                    ParameterInfo[] param = member.GetParameters();
                    StringBuilder sb = new StringBuilder();
                    foreach (ParameterInfo pm in param)
                    {
                        sb.Append(pm.ParameterType.Name + " " + pm.Name + ",");
                    }

                    //获取HTTPAttribute
                    string httpMethod = string.Empty;
                    IEnumerable<Attribute> atts = member.GetCustomAttributes();
                    foreach (Attribute a in atts)
                    {
                        if (a.GetType().Name.StartsWith("Http"))
                        {
                            httpMethod = a.GetType().Name.Replace("Http", "").Replace("Attribute", "");
                        }
                    }

                    // 处理固定接口描述
                    if (desc == string.Empty)
                    {
                        switch (member.Name)
                        {
                            case "Create":
                                desc = "创建记录";
                                break;
                            case "Update":
                                desc = "更新记录";
                                break;
                            case "Delete":
                                desc = "删除记录-物理删除单条";
                                break;
                            case "DeleteBatch":
                                desc = "删除记录-物理删除批量";
                                break;
                            case "SoftDelete":
                                desc = "删除记录-软删除单条";
                                break;
                            case "SoftDeleteBatch":
                                desc = "删除记录-软删除批量";
                                break;
                            case "GetItem":
                                desc = "得到实体-根据主键ID";
                                break;
                            case "GetList":
                                desc = "取得条件下数据(分页)";
                                break;
                            case "GetAll":
                                desc = "取得条件下数据(不分页)";
                                break;
                        }
                    }
                    string category = controller.Name.Replace("Controller", "");
                    string path = pathTemplate.Replace("[controller]", category).Replace("[action]", member.Name);
                    list.Add(new SysApiEntity()
                    {
                        Category = controllerDesc != string.Empty ? category + "-" + controllerDesc : category,
                        Url = path,
                        Title = desc,
                        Description = $"参数： {sb.ToString().Trim(',')}",
                        HttpMethod = httpMethod
                    });
                }
            }

            return await _sysApiService.GenerateApisAsync(list);
        }

        #endregion

    }
}