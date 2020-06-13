using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Services.System.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{
    [Description("缓存管理")]
    public class CacheController : ModuleBaseController
    {
        private readonly ICacheService _cacheServices;

        public CacheController(ICacheService cacheServices)
        {
            _cacheServices = cacheServices;
        }

        /// <summary>
        /// 获取缓存可以模板列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("得到所有缓存Key模板")]
        public IResponseEntity GetCacheKeyTemplates()
        {
            return _cacheServices.GetCacheKeyTemplates();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [HttpDelete]
        [Description("删除指定缓存Key模板的所有缓存")]
        public async Task<IResponseEntity> Clear(string cacheKey)
        {
            return await _cacheServices.ClearAsync(cacheKey);
        }
    }
}