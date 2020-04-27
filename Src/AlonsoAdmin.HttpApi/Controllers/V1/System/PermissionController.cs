using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{

    public class PermissionController : ModuleBaseController
    {
        private readonly ISysPermissionService _sysPermissionService;
        public PermissionController(ISysPermissionService sysPermissionService)
        {
            _sysPermissionService = sysPermissionService;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(PermissionAddRequest req)
        {
            return await _sysPermissionService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(PermissionEditRequest req)
        {
            return await _sysPermissionService.UpdateAsync(req);
        }

        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _sysPermissionService.DeleteAsync(id);
        }

        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _sysPermissionService.DeleteBatchAsync(ids);
        }

        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _sysPermissionService.SoftDeleteAsync(id);
        }



        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _sysPermissionService.SoftDeleteBatchAsync(ids);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _sysPermissionService.GetItemAsync(id);
        }

        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<PermissionFilterRequest> req)
        {
            return await _sysPermissionService.GetListAsync(req);
        }

        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(PermissionFilterRequest req)
        {
            return await _sysPermissionService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法     
        /// <summary>
        /// 为指定岗位分配权限（权限岗赋权）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> PermissionAssignPower(PermissionAssignPowerRequest req)
        {
            return await _sysPermissionService.PermissionAssignPowerAsync(req);
        }

        /// <summary>
        /// 取指定权限岗下的数据组ID集合
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetGroupIdsByPermissionId(string permissionId)
        {
            return await _sysPermissionService.GetGroupIdsByPermissionIdAsync(permissionId);
        }

        /// <summary>
        /// 取指定权限岗下的角色ID集合
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetRoleIdsByPermissionId(string permissionId)
        {
            return await _sysPermissionService.GetRoleIdsByPermissionIdAsync(permissionId);
        }
        #endregion

    }
}