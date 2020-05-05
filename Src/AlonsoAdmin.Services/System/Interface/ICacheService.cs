using AlonsoAdmin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ICacheService
    {
        /// <summary>
        /// 缓存key模板列表
        /// </summary>
        /// <returns></returns>
        IResponseEntity GetCacheKeyTemplates();

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        Task<IResponseEntity> ClearAsync(string cacheKey);
    }
}
