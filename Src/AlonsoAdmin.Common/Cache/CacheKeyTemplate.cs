using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Cache
{
    public static class CacheKeyTemplate
    {
        /// <summary>
        /// 用户权限 admin:user:用户主键:permissions
        /// </summary>
        public const string UserPermissions = "admin:user:{0}:permissions";

    }
}
