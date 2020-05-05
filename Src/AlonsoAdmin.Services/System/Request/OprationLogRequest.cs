using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{

    public class OprationLogAddRequest : SysOperationLogEntity
    {

    }


    public class OprationLogFilterRequest
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key { get; set; } 
    }
}
