using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Domain.System.Interface
{
    public interface IPermissionDomain
    {
        /// <summary>
        /// 权限岗位赋权
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="roleIds"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        Task<bool> PermissionAssignPowerAsync(string permissionId, List<string> roleIds, List<string> groupIds);

        /// <summary>
        /// 根据指定权限岗，得到权限菜单集合
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<List<SysResourceEntity>> GetPermissionMenusAsync(string permissionId);

        /// <summary>
        /// 根据指定权限岗，得到权限数据组集合 
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<List<SysGroupEntity>> GetPermissionGroupsAsync(string permissionId);

    }
}
