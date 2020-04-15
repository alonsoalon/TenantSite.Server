using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlonsoAdmin.Entities.System.Enums;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 用户
	/// </summary>
	[Table(Name = "sys_user")]
	public class SysUserEntity : BaseEntity
	{
		/// <summary>
		/// 用户类型 0普通用户 1超管
		/// </summary>
		[Column(Name = "USER_TYPE", Position = 2, MapType = typeof(int))]
		public UserType UserType { get; set; }


		/// <summary>
		/// 用户名 可用于登录
		/// </summary>
		[Column(Name = "USER_NAME", Position = 3)]
		public string UserName { get; set; } = string.Empty;

		/// <summary>
		/// 显示名称
		/// </summary>
		[Column(Name = "DISPLAY_NAME", Position = 4)]
		public string DisplayName { get; set; } = string.Empty;

		/// <summary>
		/// 手机号
		/// </summary>
		[Column(Name = "MOBILE", Position = 5)]
		public string Mobile { get; set; } = string.Empty;

		/// <summary>
		/// 密码
		/// </summary>
		[Column(Name = "PASSWORD", Position = 6)]
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// 电子邮件
		/// </summary>
		[Column(Name = "MAIL", Position = 7)]
		public string Mail { get; set; } = string.Empty;

		/// <summary>
		/// 权限ID 权限ID
		/// </summary>
		[Column(Name = "PERMISSION_ID", Position = 8)]
		public string PermissionId { get; set; }

		/// <summary>
		/// 微信OPENID 绑定微信
		/// </summary>
		[Column(Name = "WEXIN", Position = 9)]
		public string Wexin { get; set; } = string.Empty;

		/// <summary>
		/// 头像
		/// </summary>
		[MaxLength(100)]
		[Column(Name = "AVATAR", Position = 12)]
		public string Avatar { get; set; }


		#region 导航属性
		[Navigate("PermissionId")]
		public virtual SysPermissionEntity Permission { get; set; }
		#endregion
	}

}
