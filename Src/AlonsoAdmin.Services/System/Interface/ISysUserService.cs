using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysUserService
    {
        //Task<IEnumerable<string>> GetCurrentUserRolesAsync();

        //Task<ResponseEntity<UserGetResponse>> GetAsync(long id);

        Task<IResponseEntity> GetPageAsync(RequestEntity<SysUserEntity> req);

    }
}
