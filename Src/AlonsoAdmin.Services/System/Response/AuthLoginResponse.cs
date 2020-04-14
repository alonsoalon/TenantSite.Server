using AlonsoAdmin.Common.JsonConvert;
using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlonsoAdmin.Services.System.Response
{
    public class AuthLoginResponse 
    {
        
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 权限菜单
        /// </summary>
        public List<SysResourceEntity> Menus { get; set; }
    }
}
