using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysApiService : IBaseService<ApiFilterRequest, ApiAddRequest, ApiEditRequest>
    {

        #region 特殊接口 在此定义

        /// <summary>
        /// 生成APIS
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<IResponseEntity> GenerateApisAsync(List<SysApiEntity> list);

        #endregion
    }
}
