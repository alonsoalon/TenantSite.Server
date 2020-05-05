using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AlonsoAdmin.Common.Auth
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public interface IAuthUser
    {
        /// <summary>
        /// Key
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 显示名
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 权限岗Id
        /// </summary>
        string PermissionId { get; }
    }
}
