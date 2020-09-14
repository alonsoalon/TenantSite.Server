using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using AlonsoAdmin.Entities.System.Enums;
using Newtonsoft.Json.Converters;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 系统设置
	/// </summary>
	[Table(Name = "sys_config")]
	public class SysConfigEntity : BaseEntity
	{

		/// <summary>
		/// 编码
		/// </summary>
		[Column(Name = "CODE", Position = 2)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 名称
		/// </summary>
		[Column(Name = "TITLE", Position = 3)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 4)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 配置类型:系统配置(System)、部门配置（Group）、用户（User）
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[Column(Name = "CONFIG_TYPE", Position = 5, MapType = typeof(string))]
		public ConfigType ConfigType { get; set; } = ConfigType.System;

		/// <summary>
		/// 目标主体ID 如果是用户存放用户ID，如果是部门存放部门ID，如果是系统配置存SYSTEM
		/// </summary>
		[Column(Name = "TARGET_ID", Position = 6)]
		public string TargetId { get; set; } = string.Empty;

		/// <summary>
		/// 目标主体显示名称 如果是用户存放用户名，如果是部门存放部门名称，如果是系统配置存系统配置
		/// </summary>
		[Column(Name = "TARGET_LABEL", Position = 7)]
		public string TargetLabel { get; set; } = string.Empty;

		/// <summary>
		/// 数据类型 数据类型:文本、数字、日期等
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[Column(Name = "DATA_TYPE", Position = 8, MapType = typeof(string))]
		public DataType DataType { get; set; } = DataType.Text;

		/// <summary>
		/// 数据值
		/// </summary>
		[Column(Name = "DATA_VALUE", Position = 9)]
		public string DataValue { get; set; } = string.Empty;

		/// <summary>
		/// 是否公开，为True则开放给指定主体
		/// 例:如果为用户配置公开后，该用户登录后可在个人中心自行维护自己账号的配置
		/// </summary>
		[Column(Name = "IS_PUBLIC", Position = 10)]
		public bool IsPublic { get; set; } = false;

		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 11)]
		public int? OrderIndex { get; set; }

	}

}
