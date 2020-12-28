using AlonsoAdmin.Common.File;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysConfigService : IBaseService<ConfigFilterRequest, ConfigAddRequest, ConfigEditRequest>
    {

        #region 特殊接口 在此定义

        #endregion
    }
}
