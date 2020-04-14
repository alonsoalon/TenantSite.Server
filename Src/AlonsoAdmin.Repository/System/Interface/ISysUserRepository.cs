using AlonsoAdmin.Entities.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public interface ISysUserRepository : IRepositoryBase<SysUserEntity>
    {
        /// <summary>
        /// 得到当前用户的角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetCurrentUserRolesAsync();

        /// <summary>
        /// 得到当前用户有权限的Apis列表
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetCurrentUserApisAsync();

    }
}
