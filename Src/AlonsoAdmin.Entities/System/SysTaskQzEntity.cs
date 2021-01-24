using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities.System
{
    [Table(Name = "Sys_TaskQz")]
    public class SysTaskQzEntity : BaseEntity
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Column(Name = "Name")]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [Column(Name = "JobGroup")]
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [Column(Name = "Cron")]
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [Column(Name = "AssemblyName")]
        public string AssemblyName { get; set; } = "AlonsoAdmin.Tasks";
        /// <summary>
        /// 任务所在类
        /// </summary>
        [Column(Name = "ClassName")]
        public string ClassName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [Column(Name = "Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        [Column(Name = "RunTimes")]
        public int RunTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Column(Name = "BeginTime")]
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Column(Name = "EndTime")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        /// </summary>
        [Column(Name = "TriggerType")]
        public int TriggerType { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        [Column(Name = "IntervalSecond")]
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        [Column(Name = "IsStart")]
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 执行传参
        /// </summary>
        [Column(Name = "JobParams")]
        public string JobParams { get; set; }

        /// <summary>
        /// 是否是默认数据库
        /// </summary>
        [Column(Name = "IsDefaultDatabase")]
        public bool IsDefaultDatabase { get; set; }

        /// <summary>
        /// 数据库连接参数
        /// </summary>
        [Column(Name = "ConnectionParam")]
        public string ConnectionParam { get; set; }
    }

}
