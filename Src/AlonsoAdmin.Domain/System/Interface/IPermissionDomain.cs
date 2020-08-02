using AlonsoAdmin.Entities.System;
using FreeSql.Internal.Model;
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
        /// 得到权限模板资源集合
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<List<SysResourceEntity>> GetPermissionResourcesAsync(string permissionId);


        /// <summary>
        /// 得到权限模板API集合
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<List<SysApiEntity>> GetPermissionApisAsync(string permissionId);

        /// <summary>
        /// 得到权限模板数据条件集合
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        Task<List<SysConditionEntity>> GetPermissionConditionsAsync(string permissionId);

        /// <summary>
        /// 得到指定权限模板+指定模块KEY的数据条件
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        Task<DynamicFilterInfo> GetPermissionDynamicFilterAsync(string permissionId, string moduleKey);

    }
}
