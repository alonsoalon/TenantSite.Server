using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AlonsoAdmin.Common.Cache
{
    public static class CacheKeyTemplate
    {
        /// <summary>
        /// 组织机构缓存
        /// </summary>
        [Description("全局-组织机构缓存")]
        public const string GroupList = "admin:GroupList";

        /// <summary>
        /// 权限模板菜单资源缓存 admin:Permission:权限模板主键:ResourceList
        /// </summary>
        [Description("权限模板-资源集合缓存")]
        public const string PermissionResourceList = "admin:Permission:{0}:ResourceList";



        /// <summary>
        /// 权限模板Api缓存 admin:Permission:权限模板主键:ApiList
        /// </summary>
        [Description("权限模板-Api集合缓存")]
        public const string PermissionApiList = "admin:Permission:{0}:ApiList";

    }
}
