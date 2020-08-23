using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{

	public class UserSharedMember
	{

		/// <summary>
		/// 显示名称
		/// </summary>
		public string DisplayName { get; set; } = string.Empty;

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { get; set; } = string.Empty;

		/// <summary>
		/// 电子邮件
		/// </summary>
		public string Mail { get; set; } = string.Empty;

		/// <summary>
		/// 权限ID
		/// </summary>
		public string PermissionId { get; set; } = string.Empty;

		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool IsDisabled { get; set; } = false;

		/// <summary>
		/// 数据归属组
		/// </summary>
		public string GroupId { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; } = string.Empty;



	}


	/// <summary>
	/// 添加实体类 要么继承 数据库实体类，要么属性尽量取名一致(除非迁就前端)，避免automapper做对应映射处理
	/// </summary>
	public class UserAddRequest:UserSharedMember
	{

		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "用户名不能为空！")]
		public string UserName { get; set; } 
		/// <summary>
		/// 密码
		/// </summary>
		[Required(ErrorMessage = "密码不能为空！")]
		public string Password { get; set; } 


	}


	public class UserEditRequest: UserSharedMember
	{
		public string Id { get; set; }

		public string UserName { get; set; }

		public int Revision { get; set; }
	}

	public class UserFilterRequest
	{
		/// <summary>
		/// 查询关键字
		/// </summary>
		public string Key { get; set; } = string.Empty;

		/// <summary>
		/// 是否包含禁用的数据
		/// </summary>
		public bool WithDisable { get; set; } = false;
	}


	/// <summary>
	/// 当前登录用户密码修改
	/// </summary>
	public class ChangePasswordRequest
	{
		/// <summary>
		/// 旧密码
		/// </summary>
		[Required(ErrorMessage = "请输入旧密码")]
		public string OldPassword { get; set; }

		/// <summary>
		/// 新密码
		/// </summary>
		[Required(ErrorMessage = "请输入新密码")]
		public string NewPassword { get; set; }

		/// <summary>
		/// 确认新密码
		/// </summary>
		[Required(ErrorMessage = "请输入确认新密码")]
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 版本
		/// </summary>
		public int Revision { get; set; }
	}

	/// <summary>
	/// 指定用户密码修改
	/// </summary>
	public class UserChangePasswordRequest: ChangePasswordRequest
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Required(ErrorMessage = "用户主键不能为空！")]
		public string Id { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "用户名不能为空！")]
		public string UserName { get; set; }
	}

	/// <summary>
	/// 更新当前用户头像
	/// </summary>
	public class UploadAvatarRequest
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Required(ErrorMessage = "头像不能为空！")]
		public string Avatar { get; set; }

	}

}
