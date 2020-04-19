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
        Task<IResponseEntity> PageAsync(RequestEntity<SysOprationLogEntity> req);

        Task<IResponseEntity> AddAsync(OprationLogRequest req);
    }
}
