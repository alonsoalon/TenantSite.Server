using AlonsoAdmin.Entities;
using AlonsoAdmin.Services.System.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.Services.System.Interface
{
    public interface IAuthService
    {
        Task<IResponseEntity> LoginAsync(AuthLoginRequest req);

        Task<IResponseEntity> GetUserInfoAsync();

        /// <summary>
        /// 得到当前用户的权限数据组
        /// </summary>
        Task<IResponseEntity> GetUserGroupsAsync();


    }
}
