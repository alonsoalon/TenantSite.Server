using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 角色
	/// </summary>
	[Table(Name = "sys_role")]
	public partial class SysRoleEntity : BaseEntity
	{
		/// <summary>
		/// 资源CODE
		/// </summary>
		[Column(Name = "CODE", Position = 2)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 角色名称
		/// </summary>
		[Column(Name = "TITLE", Position = 3)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 角色描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 4)]
		public string Description { get; set; } = string.Empty;


		/// <summary>
		/// 排序 
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 5)]
		[MaxValue]
		public int? OrderIndex { get; set; }


		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRPermissionRoleEntity))]
		public virtual ICollection<SysPermissionEntity> Permissions { get; set; }


		[Navigate(ManyToMany = typeof(SysRRoleResourceEntity))]
		public virtual ICollection<SysResourceEntity> Resources { get; set; }
		#endregion

	}

}
