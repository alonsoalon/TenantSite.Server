using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysResourceService : IBaseService<ResourceFilterRequest, ResourceAddRequest, ResourceEditRequest>
    {

        // 特殊接口 在此定义

        /// <summary>
        /// 得到资源列表 - 合并功能到菜单资源
        /// </summary>
        /// <returns></returns>
        Task<IResponseEntity> GetResourcesAsync();

        /// <summary>
        /// 得到资源列表 - 根据角色ID
        /// </summary>
        /// <returns></returns>
        Task<IResponseEntity> GetResourceIdsByRoleIdAsync(string roleId);
        
    }
}
