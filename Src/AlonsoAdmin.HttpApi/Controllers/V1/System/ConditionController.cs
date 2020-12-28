using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{
    [Description("数据条件")]
    public class ConditionController : ModuleBaseController
    {
        private readonly ISysConditionService _sysConditionService;
        public ConditionController(ISysConditionService sysConditionService)
        {
            _sysConditionService = sysConditionService;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(ConditionAddRequest req)
        {
            return await _sysConditionService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(ConditionEditRequest req)
        {
            return await _sysConditionService.UpdateAsync(req);
        }

        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _sysConditionService.DeleteAsync(id);
        }

        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _sysConditionService.DeleteBatchAsync(ids);
        }

        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _sysConditionService.SoftDeleteAsync(id);
        }



        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _sysConditionService.SoftDeleteBatchAsync(ids);

        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _sysConditionService.GetItemAsync(id);
        }

        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<ConditionFilterRequest> req)
        {
            return await _sysConditionService.GetListAsync(req);
        }

        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(ConditionFilterRequest req)
        {
            return await _sysConditionService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法
        
        #endregion

    }
}