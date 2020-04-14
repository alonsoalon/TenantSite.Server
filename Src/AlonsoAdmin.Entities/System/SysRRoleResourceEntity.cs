using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 资源与API关系表
	/// </summary>
	[Table(Name = "sys_r_role_resource")]
	[Index("uk_sys_r_role_resource_roleid_resourceid", "RoleId,ResourceId", true)]
	public class SysRRoleResourceEntity : BaseIdEntity
	{

		/// <summary>
		/// ROLE_ID
		/// </summary>
		[Column(Name = "ROLE_ID", Position = 2)]
		public long RoleId { get; set; }


		/// <summary>
		/// 资源ID
		/// </summary>
		[Column(Name = "RESOURCE_ID", Position = 3)]
		public long ResourceId { get; set; }

		#region 导航属性

		/// <summary>
		/// 角色
		/// </summary>
		[Navigate("RoleId")]
		public virtual SysRoleEntity Role { get; set; }

		/// <summary>
		/// 资源
		/// </summary>
		[Navigate("ResourceId")]
		public virtual SysResourceEntity Resource { get; set; }

		#endregion
	}

}
