using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlonsoAdmin.Common.Cache
{
    public static class CacheKeyTemplate
    {

        /// <summary>
        /// 权限岗菜单资源缓存 admin:Permission:权限岗主键:MenuList
        /// </summary>
        [Description("权限岗菜单资源缓存")]
        public const string PermissionMenuList = "admin:Permission:{0}:MenuList";

        /// <summary>
        /// 权限岗权限组缓存 admin:Permission:权限岗主键:GroupList
        /// </summary>
        [Description("权限岗权限组缓存")]
        public const string PermissionGroupList = "admin:Permission:{0}:GroupList";

    }
}
