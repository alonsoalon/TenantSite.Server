using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysRoleService : IBaseService<RoleFilterRequest, RoleAddRequest, RoleEditRequest>
    {

        // 特殊接口 在此定义

        /// <summary>
        /// 为角色分配资源（角色赋权）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> RoleAssignResourcesAsync(RoleResourceAssignRequest req);

        /// <summary>
        /// 根据指定角色ID取回该角色的资源ID集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IResponseEntity> GetResourceIdsByIdAsync(string roleId);
    }
}
