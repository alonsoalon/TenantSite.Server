
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 数据组/组织机构
	/// </summary>
	[Table(Name = "sys_group")]
	public class SysGroupEntity : BaseEntity
	{
		/// <summary>
		/// 数据组编码 数据组编码
		/// </summary>
		[Column(Name = "CODE", Position = 2)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 数据组标题 数据组标题
		/// </summary>
		[Column(Name = "TITLE", Position = 3)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 数据组描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 4)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 父级ID
		/// </summary>
		[Column(Name = "PARENT_ID", Position = 5)]
		public string ParentId { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 6)]
		[MaxValue]
		public int? OrderIndex { get; set; }

		/// <summary>
		/// 打开状态
		/// </summary>
		[Column(Name = "OPENED", Position = 7)]
		public bool? Opened { get; set; }

		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRPermissionGroupEntity))]
		public virtual ICollection<SysPermissionEntity> Permissions { get; set; }
		#endregion

	}

}
