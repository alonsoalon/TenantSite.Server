
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 权限与数据条件关系
	/// </summary>
	[Table(Name = "sys_r_permission_condition")]
	[Index("uk_sys_r_permission_condition_permissionid_conditionid", "PermissionId,ConditionId", true)]
	public class SysRPermissionConditionEntity : BaseIdEntity
	{
		/// <summary>
		/// 权限ID
		/// </summary>
		[Column(Name = "PERMISSION_ID", Position = 2)]
		public string PermissionId { get; set; }

		/// <summary>
		/// 数据条件ID
		/// </summary>
		[Column(Name = "CONDITION_ID", Position = 3)]
		public string ConditionId { get; set; } 


		#region 导航属性
		/// <summary>
		/// 权限
		/// </summary>
		[Navigate("PermissionId")]
		public virtual SysPermissionEntity Permission { get; set; }

		/// <summary>
		/// 数据条件
		/// </summary>
		[Navigate("ConditionId")]
		public virtual SysConditionEntity Condition { get; set; }
		#endregion

	}

}
