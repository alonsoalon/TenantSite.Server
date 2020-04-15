
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlonsoAdmin.Services.System.Response
{
    
    public class UserListResponse:SysUserEntity
    {

        /// <summary>
        /// 权限岗名称
        /// </summary>
        public string PermissionName { get; set; }



    }
}
