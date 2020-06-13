using AlonsoAdmin.Common.RequestEntity;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.System.Request;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysLoginLogService
    {
        /// <summary>
        /// 登录日志-取得分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> GetListAsync(RequestEntity<LoginLogFilterRequest> req);


        /// <summary>
        /// 登录日志-新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> CreateAsync(LoginLogAddRequest req);
    }
}
