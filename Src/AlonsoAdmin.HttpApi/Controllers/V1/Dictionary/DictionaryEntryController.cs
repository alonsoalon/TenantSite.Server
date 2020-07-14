using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.Dictionary.Interface;
using AlonsoAdmin.Services.Dictionary.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.Dictionary
{
    [Description("字典管理")]
    public class DictionaryEntryController : ModuleBaseController
    {
        private readonly IDictionaryEntryService _dictionaryDetailService;
        public DictionaryEntryController(IDictionaryEntryService dictionaryService)
        {
            _dictionaryDetailService = dictionaryService;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(DictionaryEntryAddRequest req)
        {
            return await _dictionaryDetailService.CreateAsync(req);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(DictionaryEntryEditRequest req)
        {
            return await _dictionaryDetailService.UpdateAsync(req);
        }
        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _dictionaryDetailService.DeleteAsync(id);
        }
        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _dictionaryDetailService.DeleteBatchAsync(ids);
        }
        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _dictionaryDetailService.SoftDeleteAsync(id);
        }
        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _dictionaryDetailService.SoftDeleteBatchAsync(ids);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _dictionaryDetailService.GetItemAsync(id);
        }
        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<DictionaryEntryFilterRequest> req)
        {
            return await _dictionaryDetailService.GetListAsync(req);
        }
        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(DictionaryEntryFilterRequest req)
        {
            return await _dictionaryDetailService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法

        #endregion
    }
}