
using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlonsoAdmin.Services.System.Response
{
    public class AuthLoginResponse : AuthResourceResponse
    {
        
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 权限岗ID
        /// </summary>
        public string PermissionId { get; set; }
        

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

    }

    public class AuthResourceResponse
    {

        /// <summary>
        /// 权限菜单
        /// </summary>
        public List<ResourceForMenuResponse> Menus { get; set; }

        /// <summary>
        /// 功能权限列表
        /// </summary>
        public List<string> FunctionPoints { get; set; }
    }

}
