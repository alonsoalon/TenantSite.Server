using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysUserService
    {
        Task<IEnumerable<string>> GetCurrentUserRolesAsync();
    }
}
