using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlonsoAdmin.Common.Cache
{
    public static class CacheKeyTemplate
    {

        /// <summary>
        /// 用户权限 admin:user:用户主键:permissions
        /// </summary>
        [Description("用户权限")]
        public const string UserPermissionList = "admin:user:{0}:permissionlist";

        /// <summary>
        /// 用户权限组
        /// </summary>
        [Description("用户权限组")]
        public const string UserGroupList = "admin:user:{0}:grouplist";

    }
}
