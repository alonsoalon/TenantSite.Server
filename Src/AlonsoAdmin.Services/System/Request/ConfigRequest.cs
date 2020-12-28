using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{
	/// <summary>
	/// 添加实体类 要么继承 数据库实体类，要么属性尽量取名一致(除非迁就前端)，避免automapper做对应映射处理
	/// </summary>
	public class ConfigAddRequest
    {

		/// <summary>
		/// 编码
		/// </summary>
		[Required(ErrorMessage = "Code不能为空！")]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 名称
		/// </summary>
		[Required(ErrorMessage = "标题不能为空！")]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 配置类型:系统配置(SYSTEM)、权限配置（PERMISSION）、用户（USER）
		/// </summary>
		public string ConfigType { get; set; } = string.Empty;

		/// <summary>
		/// 目标主体ID 如果是用户存放用户ID，如果是部门存放部门ID，如果是系统配置存SYSTEM
		/// </summary>
		public string TargetId { get; set; } = string.Empty;

		/// <summary>
		/// 目标主体显示名称 如果是用户存放用户名，如果是部门存放部门名称，如果是系统配置存系统配置
		/// </summary>
		public string TargetLabel { get; set; } = string.Empty;

		/// <summary>
		/// 数据类型 数据类型:文本、数字、日期等
		/// </summary>
		public Entities.System.Enums.DataType DataType { get; set; } = Entities.System.Enums.DataType.Text;

		/// <summary>
		/// 数据值
		/// </summary>
		public string DataValue { get; set; } = string.Empty;

		/// <summary>
		/// 是否公开，为True则开放给指定主体
		/// 例:如果为用户配置公开后，该用户登录后可在个人中心自行维护自己账号的配置
		/// </summary>
		public string IsPublic { get; set; } = string.Empty;


		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool IsDisabled { get; set; } = false;


	}

	public class ConfigEditRequest : ConfigAddRequest
	{
		public string Id { get; set; }

		public int? OrderIndex { get; set; }

		public int Revision { get; set; }

	}

	public class ConfigFilterRequest
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


	// 其他请求实体


}
