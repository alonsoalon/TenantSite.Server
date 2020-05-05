using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities.System
{

    [Table(Name = "sys_operation_log")]
    public class SysOperationLogEntity : LogAbstract
    {

        /// <summary>
        /// API标题
        /// </summary>
        [Column(Name = "API_TITLE", Position = 11, StringLength = 500)]
        public string ApiTitle { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        [Column(Name = "API_PATH", Position = 12, StringLength = 500)]
        public string ApiPath { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>        
        [Column(Name = "API_METHOD", Position = 13, StringLength = 50)]
        public string ApiMethod { get; set; }

        /// <summary>
        /// 操作参数
        /// </summary>      
        [Column(Name = "PARAMS", Position = 14, StringLength = -1)]
        public string Params { get; set; }
    }
}
