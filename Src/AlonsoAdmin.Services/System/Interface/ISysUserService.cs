using AlonsoAdmin.Common.File;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface ISysUserService : IBaseService<UserFilterRequest, UserAddRequest, UserEditRequest>
    {

        #region 特殊接口 在此定义

        /// <summary>
        /// 修改指定用户的密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> UserChangePasswordAsync(UserChangePasswordRequest req);

        /// <summary>
        /// 修改当前用户的密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> ChangePasswordAsync(ChangePasswordRequest req);


        /// <summary>
        /// 更新当前用户头像
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity<FileInfo>> UploadAvatarAsync(IFormFile file);

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<IResponseEntity> UpdateUserInfo(UserEditRequest req);


        #endregion
    }
}
