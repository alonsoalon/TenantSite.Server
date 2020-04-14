using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlonsoAdmin.Entities
{

	/// <summary>
	/// 只需要插入数据，但无需编辑数据的实例类继承此类，如日志等
	/// 包括了ID、创建人相关
	/// </summary>
    public abstract class BaseAddEntity : BaseIdEntity
    {
		#region 创建人相关

		/// <summary>
		/// 创建人
		/// </summary>
		[Column(Name = "CREATED_BY", Position = -7, CanUpdate = false)]
		public long? CreatedBy { get; set; }

		/// <summary>
		/// 创建人名称
		/// </summary>
		[Column(Name = "CREATED_BY_NAME", Position = -6, CanUpdate = false), MaxLength(50)]
		public string CreatedByName { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Column(Name = "CREATED_TIME", Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local)]
		public DateTime? CreatedTime { get; set; }

		#endregion 创建人相关
	}
}
