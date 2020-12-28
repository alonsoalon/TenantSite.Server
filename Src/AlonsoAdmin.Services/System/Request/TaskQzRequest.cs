using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{
    /// <summary>
    /// 添加实体类 要么继承 数据库实体类，要么属性尽量取名一致(除非迁就前端)，避免automapper做对应映射处理
    /// </summary>
    public class TaskQzAddRequest
    {

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int? RunTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        /// </summary>
        public int TriggerType { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int? IntervalSecond { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; }
        /// <summary>
        /// 执行传参
        /// </summary>
        public string JobParams { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 是否是默认数据库
        /// </summary>
        public bool IsDefaultDatabase { get; set; }

        /// <summary>
        /// 数据库连接参数
        /// </summary>
        public string ConnectionParam { get; set; }
    }

    public class TaskQzEditRequest : TaskQzAddRequest
    {
        public string Id { get; set; }

        public int Revision { get; set; }

    }

    public class TaskQzFilterRequest
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 是否包含禁用的数据
        /// </summary>
        public bool WithDisable { get; set; } = false;
    }

}
