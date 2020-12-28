using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 数据条件
	/// </summary>
	[Table(Name = "sys_condition")]
	public class SysConditionEntity : BaseEntity
	{
		/// <summary>
		/// 编码
		/// </summary>
		[Column(Name = "CODE", Position = 2)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 标题
		/// </summary>
		[Column(Name = "TITLE" , Position = 3)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column(Name = "DESCRIPTION",  Position = 4)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 表达式树(SQL)
		/// </summary>
		[Column(Name = "EXPRESSION", Position = 5)]
		public string Expression { get; set; } = string.Empty;

		/// <summary>
		/// 动态条件
		/// </summary>
		[Column(Name = "CONDITION", Position = 6)]
		public string Condition { get; set; } = string.Empty;


		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 7)]
		[MaxValue]
		public int? OrderIndex { get; set; }


		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRPermissionConditionEntity))]
		public virtual ICollection<SysPermissionEntity> Permissions { get; set; }
		#endregion

	}

}
