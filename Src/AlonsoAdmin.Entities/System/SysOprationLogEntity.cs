using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities.System
{

    [Table(Name = "sys_opration_log")]
    public class SysOprationLogEntity : LogAbstract
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        [Column(Name = "API_PATH", Position = 11, StringLength = 500)]
        public string ApiPath { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>        ]
        [Column(Name = "API_METHOD", Position = 12, StringLength = 50)]
        public string ApiMethod { get; set; }

        /// <summary>
        /// 操作参数
        /// </summary>      
        [Column(Name = "PARAMS", Position = 13, StringLength = -1)]
        public string Params { get; set; }
    }
}
