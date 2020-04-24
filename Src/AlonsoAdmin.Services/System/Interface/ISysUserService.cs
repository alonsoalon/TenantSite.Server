using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Request;
using AlonsoAdmin.Services.System.Response;
using System;
using System.Collections.Generic;
using System.Text;
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
        #endregion
    }
}
