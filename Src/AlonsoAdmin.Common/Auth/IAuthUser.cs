using AlonsoAdmin.MultiTenant;
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
        /// 权限模板Id
        /// </summary>
        string PermissionId { get; }

        /// <summary>
        /// 所在部门
        /// </summary>
        string GroupId { get; }

        TenantInfo Tenant { get; }
    }
}
