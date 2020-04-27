using AlonsoAdmin.Entities.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public interface ISysPermissionRepository : IRepositoryBase<SysPermissionEntity>
    {
        /// <summary>
        /// 权限岗位赋权
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="roleIds"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        Task<bool> PermissionAssignPowerAsync(string permissionId, List<string> roleIds, List<string> groupIds);
    }
}
