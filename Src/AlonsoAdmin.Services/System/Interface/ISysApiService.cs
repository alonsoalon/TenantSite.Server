using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysApiService : IBaseService<ApiFilterRequest, ApiAddRequest, ApiEditRequest>
    {

        #region 特殊接口 在此定义

        Task<IResponseEntity> GenerateApisAsync(List<SysApiEntity> list);

        #endregion
    }
}
