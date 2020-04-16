using AlonsoAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System
{
    public interface IBaseService<TFilter, TAddEntity,TEditEntity>
        where TFilter : class     // 用于前端 查询条件请求实体
        where TAddEntity : class  // 用于前端 新增请求实体
        where TEditEntity : class // 用于前端 更新请求实体
    {

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseEntity> DeleteAsync(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseEntity> DeleteBatchAsync(string[] ids);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseEntity> SoftDeleteAsync(string id);

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseEntity> SoftDeleteBatchAsync(string[] ids);
        #endregion

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> CreateAsync(TAddEntity req);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> UpdateAsync(TEditEntity req);

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> GetListAsync(RequestEntity<TFilter> req);


        /// <summary>
        /// 查询所有分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> GetAllAsync(TFilter req);

    }
}
