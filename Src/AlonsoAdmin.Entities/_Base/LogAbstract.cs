using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlonsoAdmin.Entities
{
    /// <summary>
    /// 日志类
    /// </summary>
    public abstract class LogAbstract: BaseAddEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        /// 
        [MaxLength(60)]
        [Column(Name = "REAL_NAME", Position = 2 )]
        public string RealName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [MaxLength(100)]
        [Column(Name = "IP", Position = 3)]
        public string Ip { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [MaxLength(100)]
        [Column(Name = "BROWSER", Position = 4)]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [MaxLength(100)]
        [Column(Name = "OS", Position = 5)]
        public string Os { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [MaxLength(50)]
        [Column(Name = "DEVICE", Position = 6)]
        public string Device { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
       
        [Column(Name = "BROWSERINFO", Position = 7,StringLength = -1)]
        public string BrowserInfo { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        [Column(Name = "ELAPSED_MILLISECONDS", Position = 8)]
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        [Column(Name = "STATUS", Position = 9)]
        public bool Status { get; set; }

        /// <summary>
        /// 操作消息
        /// </summary>
        
        [Column(Name = "MESSAGE", Position = 10, StringLength = 500)]
        public string Message { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        [Column(Name = "RESULT", Position = 10, StringLength = -1)]        
        public string Result { get; set; }
    }
}
