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
		/// 标题
		/// </summary>
		[Column(Name = "TITLE" , Position = 2)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column(Name = "DESCRIPTION",  Position = 3)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 表达式树 表达式树,目前存放SQL语句
		/// </summary>
		[Column(Name = "EXPRESSION", Position = 4)]
		public string Expression { get; set; } = string.Empty;


		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 5)]
		public int? OrderIndex { get; set; }


		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRPermissionConditionEntity))]
		public virtual ICollection<SysPermissionEntity> Permissions { get; set; }
		#endregion

	}

}
