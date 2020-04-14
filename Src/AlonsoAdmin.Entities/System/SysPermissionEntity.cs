using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 权限
	/// </summary>
	[Table(Name = "sys_permission")]
	public class SysPermissionEntity : BaseEntity
	{

		/// <summary>
		/// 权限标题
		/// </summary>
		[Column(Name = "TITLE", Position = 2)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 权限描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 3)]
		public string Description { get; set; } = string.Empty;



        #region 导航属性
        [Navigate(ManyToMany = typeof(SysRPermissionRoleEntity))]
		public virtual ICollection<SysRoleEntity> Roles { get; set; }

		[Navigate(ManyToMany = typeof(SysRPermissionGroupEntity))]
		public virtual ICollection<SysGroupEntity> Groups { get; set; }

		[Navigate(ManyToMany = typeof(SysRPermissionConditionEntity))]
		public virtual ICollection<SysConditionEntity> Conditions { get; set; }

		[Navigate("Id")]
		public virtual ICollection<SysUserEntity> Users { get; set; }
		#endregion

	}

}
