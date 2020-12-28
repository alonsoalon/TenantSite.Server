using System;
using System.Text;
using System.Collections.Generic;
using AlonsoAdmin.Entities.System;

namespace AlonsoAdmin.Services.System.Response
{
	/// <summary>
	/// ForList 实体对象（一般用于列表页展示数据用）
	/// </summary>
	public class TaskQzForListResponse : SysTaskQzEntity
	{

		/// <summary>
		/// 任务运行状态
		/// </summary>
		public string TriggerState { get; set; }

		/// <summary>
		/// 下一次执行时间
		/// </summary>
		public DateTime? NextFireTime { get; set; }
	}

	/// <summary>
	/// ForIem 实体对象（一般用于明细页展示数据用）
	/// </summary>
	public class TaskQzForItemResponse : TaskQzForListResponse
	{

	}

}
