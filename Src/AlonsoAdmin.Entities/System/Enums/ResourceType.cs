using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities.System.Enums
{
    public enum ResourceType
    {
        /// <summary>
        /// 菜单分组
        /// </summary>
        Group = 1,
        /// <summary>
        /// 菜单
        /// </summary>
        Menu = 2,
        /// <summary>
        /// 功能点（功能点包括 按钮，显示区域，具体由前端控制）
        /// </summary>
        Func = 3
    }
}
