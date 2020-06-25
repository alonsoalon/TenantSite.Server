using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Common.Utils;
using AlonsoAdmin.HttpApi.Init.Services;
using AlonsoAdmin.HttpApi.Init.Utils;
using AlonsoAdmin.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Init.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        ISettingService _settingService;
        public ConfigController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        /// <summary>
        /// 得到启动参数配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetStartupConfig()
        {
            var res = await _settingService.GetStartupSettingsAsync();
            if (res != null) {
                return ResponseEntity.Ok(res);
            }
            return ResponseEntity.Error("没取到任何数据");
        }

        /// <summary>
        /// 取得系统应用参数配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetSystemConfig()
        {
            var res = await _settingService.GetSystemSettingsAsync();
            if (res != null)
            {
                return ResponseEntity.Ok(res);
            }
            return ResponseEntity.Error("没取到任何数据");
        }

        /// <summary>
        /// 更新启动参数配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> UpdateStartupConfig(StartupConfig req)
        {
            
            var result = await _settingService.WriteConfig<StartupConfig>("Startup", opt =>
            {
                DeepCopy.CopyToAll(req, opt);
            });

            return ResponseEntity.Result(result);
        }

        /// <summary>
        /// 更新系统应用参数配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> UpdateSystemConfig(SystemConfig req)
        {

            var result = await _settingService.WriteConfig<SystemConfig>("System", opt =>
            {
                DeepCopy.CopyToAll(req, opt);
            });

            return ResponseEntity.Result(result);
        }


        /// <summary>
        /// 取得租户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetTenantList()
        {
            var res = await _settingService.GetTenantListAsync();
            if (res != null)
            {
                var obj = res.OrderBy(x => x.Id);
                return ResponseEntity.Ok(obj);
            }
            return ResponseEntity.Error("没取到任何数据");
        }

        /// <summary>
        /// 更新租户配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> SaveTenantConfig(TenantInfo req)
        {
            var tenants = await _settingService.GetTenantListAsync();

            if (req.Code == null || req.Code == "") {
                return ResponseEntity.Error("租户编码不能为空");
            }

            if (req.Id == null || req.Id == "") // 新增租户
            {
                var item = tenants.Where(x => x.Code == req.Code).FirstOrDefault();
                if (item != null)
                {
                    return ResponseEntity.Error("添加失败，租户编码已经存在");
                }

                req.Id = IdHelper.GenSnowflakeId().ToString();
            }
            else // 编辑租户
            {
                var item = tenants.Where(x => x.Id == req.Id).FirstOrDefault();
                if (item != null)
                {
                    tenants.Remove(item);
                }
            }

            #region 字典里的KEY，大小写问题
            var audience = (string)req.Items["audience"];
            var expirationMinutes = (string)req.Items["expirationMinutes"];
            var issuer = (string)req.Items["issuer"];
            var secret = (string)req.Items["secret"];

            req.Items.Clear();
            req.Items.Add("Audience", audience);
            req.Items.Add("Issuer", issuer);
            req.Items.Add("ExpirationMinutes", expirationMinutes);
            req.Items.Add("Secret", secret);
            #endregion

            tenants.Add(req);
            var updateTenants = tenants.OrderBy(x => x.Id).ToList();
            var result = await _settingService.WriteTenantsConfig(updateTenants);
            return ResponseEntity.Result(result);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> DeleteTenant(string id) {
            var tenants = await _settingService.GetTenantListAsync();
            var item = tenants.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                tenants.Remove(item);
            }
            else {
                return ResponseEntity.Error("删除失败，没有找到租户信息");
            }
            var result = await _settingService.WriteTenantsConfig(tenants);
            return ResponseEntity.Result(result);
        }


    }
}
