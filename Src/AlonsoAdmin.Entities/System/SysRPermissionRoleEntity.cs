
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 权限与角色关系
	/// </summary>
	[Table(Name = "sys_r_permission_role")]
	[Index("uk_sys_r_permission_role_permissionid_roleid", "PermissionId,RoleId", true)]
	public class SysRPermissionRoleEntity : BaseIdEntity
	{
		/// <summary>
		/// 权限ID
		/// </summary>
		[Column(Name = "PERMISSION_ID",Position = 2)]
		public long PermissionId { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
		[Column(Name = "ROLE_ID",Position = 3)]
		public long RoleId { get; set; } 


		#region 导航属性

		/// <summary>
		/// 权限
		/// </summary>
		[Navigate("PermissionId")]
		public virtual SysPermissionEntity Permission { get; set; }

		/// <summary>
		/// 角色
		/// </summary>
		[Navigate("RoleId")]
		public virtual SysRoleEntity Role { get; set; }

        #endregion
    }

}
