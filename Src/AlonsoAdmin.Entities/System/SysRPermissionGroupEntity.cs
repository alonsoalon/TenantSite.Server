using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 权限与数据组关系
	/// </summary>
	[Table(Name = "sys_r_permission_group")]
	[Index("uk_sys_r_permission_group_permissionid_groupid", "PermissionId,GroupId", true)]
	public class SysRPermissionGroupEntity : BaseIdEntity
	{

		/// <summary>
		/// 权限ID
		/// </summary>
		[Column(Name = "PERMISSION_ID", Position = 2)]
		public string PermissionId { get; set; } 

		/// <summary>
		/// 组ID
		/// </summary>
		[Column(Name = "GROUP_ID", Position = 3)]
		public string GroupId { get; set; }



		#region 导航属性
		/// <summary>
		/// 权限
		/// </summary>
		[Navigate("PermissionId")]
		public virtual SysPermissionEntity Permission { get; set; }

		/// <summary>
		/// 权限组
		/// </summary>
		[Navigate("GroupId")]
		public virtual SysGroupEntity Group { get; set; }
		#endregion

	}

}
