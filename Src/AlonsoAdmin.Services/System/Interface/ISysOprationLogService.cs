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
    public interface ISysOprationLogService
    {
        /// <summary>
        /// Api访问日志-取得分页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> GetListAsync(RequestEntity<OprationLogFilterRequest> req);

        /// <summary>
        /// Api访问日志-新增
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> CreateAsync(OprationLogAddRequest req);
    }
}
