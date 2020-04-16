using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{
    public class GroupEditRequest : GroupAddRequest
    {
        public string Id { get; set; }

        public int? OrderIndex { get; set; }

        public int Revision { get; set; }
        
        /// <summary>
        /// 是否展开显示
        /// </summary>
        public bool? Opened { get; set; }

    }
}
