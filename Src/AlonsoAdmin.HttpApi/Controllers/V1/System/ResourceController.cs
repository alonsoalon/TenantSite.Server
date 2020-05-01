using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Description("资源")]
    public class ResourceController : ModuleBaseController
    {
        private readonly ISysResourceService _sysResourceService;
        public ResourceController(ISysResourceService sysResourceService)
        {
            _sysResourceService = sysResourceService;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(ResourceAddRequest req)
        {
            return await _sysResourceService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(ResourceEditRequest req)
        {
            return await _sysResourceService.UpdateAsync(req);
        }

        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _sysResourceService.DeleteAsync(id);
        }

        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _sysResourceService.DeleteBatchAsync(ids);
        }

        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _sysResourceService.SoftDeleteAsync(id);
        }



        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _sysResourceService.SoftDeleteBatchAsync(ids);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _sysResourceService.GetItemAsync(id);
        }

        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<ResourceFilterRequest> req)
        {
            return await _sysResourceService.GetListAsync(req);
        }

        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(ResourceFilterRequest req)
        {
            return await _sysResourceService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法
        /// <summary>
        /// 根据指定资源ID获取该资源的API集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("根据指定资源ID获取该资源的API集合")]
        public async Task<IResponseEntity> GetResourceApisById(string resourceId)
        {
            return await _sysResourceService.GetResourceApisByIdAsync(resourceId);
        }

        /// <summary>
        /// 根据指定资源ID更新该资源的API集合
        /// </summary>
        /// <param name="req">UpdateResourceApiRequest</param>
        /// <returns></returns>
        [HttpPut]
        [Description("更新指定资源ID下的API集合")]
        public async Task<IResponseEntity> UpdateResourceApisById(UpdateResourceApiRequest req)
        {
            return await _sysResourceService.UpdateResourceApisByIdAsync(req);
        }

        /// <summary>
        /// 得到供配置角色的资源列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("得到供配置角色的资源列表(功能点合并)")]
        public async Task<IResponseEntity> GetResources()
        {
            return await _sysResourceService.GetResourcesAsync();
        }

    

        #endregion

    }
}