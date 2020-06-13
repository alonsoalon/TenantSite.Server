using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.System.Request;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface IAuthService
    {
        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> LoginAsync(AuthLoginRequest req);

        /// <summary>
        /// 得到当前用户信息
        /// </summary>
        /// <returns></returns>
        Task<IResponseEntity> GetUserInfoAsync();

        /// <summary>
        /// 得到当前用户的权限数据组
        /// </summary>
        Task<IResponseEntity> GetUserGroupsAsync();

        /// <summary>
        /// 验证当前用户API访问权限
        /// </summary>
        /// <param name="api">api</param>
        /// <returns></returns>
        Task<bool> VerifyUserAccessApiAsync(string api);

    }
}
