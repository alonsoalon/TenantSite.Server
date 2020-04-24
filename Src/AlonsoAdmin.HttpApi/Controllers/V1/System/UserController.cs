using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.HttpApi.SwaggerHelper;
using AlonsoAdmin.MultiTenant.Extensions;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{

    public class UserController : ModuleBaseController
    {
        
        private readonly ISysUserService _sysUserService;

        public UserController(ISysUserService sysUserServices)
        {
            _sysUserService = sysUserServices;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(UserAddRequest req)
        {
            return await _sysUserService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(UserEditRequest req)
        {
            return await _sysUserService.UpdateAsync(req);
        }

        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _sysUserService.DeleteAsync(id);
        }

        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _sysUserService.DeleteBatchAsync(ids);
        }

        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _sysUserService.SoftDeleteAsync(id);
        }



        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _sysUserService.SoftDeleteBatchAsync(ids);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _sysUserService.GetItemAsync(id);
        }

        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<UserFilterRequest> req)
        {
            return await _sysUserService.GetListAsync(req);
        }

        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(UserFilterRequest req)
        {
            return await _sysUserService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> ChangePassword(ChangePasswordRequest req) {
            return await _sysUserService.ChangePasswordAsync(req);
        }

        /// <summary>
        /// 修改指定用户密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> UserChangePassword(UserChangePasswordRequest req)
        {
            return await _sysUserService.UserChangePasswordAsync(req);
        }
        #endregion
    }
}