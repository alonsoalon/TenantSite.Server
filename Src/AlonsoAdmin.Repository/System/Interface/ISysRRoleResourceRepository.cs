using AlonsoAdmin.Entities.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public interface ISysRRoleResourceRepository : IRepositoryBase<SysRRoleResourceEntity>
    {

        /// <summary>
        /// 为角色分配资源（角色赋权）
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="resourceIds">资源ID集</param>
        /// <returns></returns>
        Task<bool> RoleAssignResourcesAsync(string roleId, List<string> resourceIds);
    }
}
