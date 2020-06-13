using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.System.Request;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysResourceService : IBaseService<ResourceFilterRequest, ResourceAddRequest, ResourceEditRequest>
    {

        #region 特殊接口 在此定义

        /// <summary>
        /// 根据指定资源ID获取该资源的API集合
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IResponseEntity> GetResourceApisByIdAsync(string resourceId);

        /// <summary>
        /// 得到资源列表 - 合并功能到菜单资源
        /// </summary>
        /// <returns></returns>
        Task<IResponseEntity> GetResourcesAsync();

        /// <summary>
        /// 根据指定资源ID更新该资源的API集合
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> UpdateResourceApisByIdAsync(UpdateResourceApiRequest req);

        #endregion

    }
}
