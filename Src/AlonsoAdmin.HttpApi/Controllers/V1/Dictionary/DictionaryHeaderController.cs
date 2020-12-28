using System.ComponentModel;
using System.Threading.Tasks;
using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.Dictionary.Interface;
using AlonsoAdmin.Services.Dictionary.Request;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.Dictionary
{
    [Description("字典管理")] 
    public class DictionaryHeaderController : ModuleBaseController
    {
        private readonly IDictionaryHeaderService _dictionaryService;
        public DictionaryHeaderController(IDictionaryHeaderService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        #region 通用API方法
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(DictionaryHeaderAddRequest req)
        {
            return await _dictionaryService.CreateAsync(req);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(DictionaryHeaderEditRequest req)
        {
            return await _dictionaryService.UpdateAsync(req);
        }
        /// <summary>
        /// 物理删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {

            return await _dictionaryService.DeleteAsync(id);
        }
        /// <summary>
        /// 物理删除 - 批量 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> DeleteBatch(string[] ids)
        {
            return await _dictionaryService.DeleteBatchAsync(ids);
        }
        /// <summary>
        /// 软删除 - 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
            return await _dictionaryService.SoftDeleteAsync(id);
        }
        /// <summary>
        /// 软删除 - 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> SoftDeleteBatch(string[] ids)
        {
            return await _dictionaryService.SoftDeleteBatchAsync(ids);
        }
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseEntity> GetItem(string id)
        {
            return await _dictionaryService.GetItemAsync(id);
        }
        /// <summary>
        /// 取得条件下分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<DictionaryHeaderFilterRequest> req)
        {
            return await _dictionaryService.GetListAsync(req);
        }
        /// <summary>
        /// 取得条件下所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(DictionaryHeaderFilterRequest req)
        {
            return await _dictionaryService.GetAllAsync(req);
        }
        #endregion

        #region 特殊API方法
        // 特殊API方法需定义特性Description，以方便程序自动生成API记录的描述，通用方法无需定义特性Description，如果定义将覆盖默认
        #endregion
    }
}