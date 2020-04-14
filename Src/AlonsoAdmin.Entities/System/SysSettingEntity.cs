using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 系统设置
	/// </summary>
	[Table(Name = "sys_setting")]
	public class SysSettingEntity : BaseEntity
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
		/// 配置类型:系统配置(SYSTEM)、权限配置（PERMISSION）、用户（USER）
		/// </summary>
		[Column(Name = "CONFIG_TYPE", Position = 5)]
		public string ConfigType { get; set; } = string.Empty;

		/// <summary>
		/// 主体 如果是用户存放用户名，如果是权限岗位存放权限ID，如果是系统存SYSTEM
		/// </summary>
		[Column(Name = "TARGET", Position = 6)]
		public string Target { get; set; } = string.Empty;

		/// <summary>
		/// 数据类型 数据类型:文本、数字、日期等
		/// </summary>
		[Column(Name = "DATA_TYPE", Position = 7)]
		public string DataType { get; set; } = string.Empty;

		/// <summary>
		/// 数据值
		/// </summary>
		[Column(Name = "DATA_VALUE", Position = 8)]
		public string DataValue { get; set; } = string.Empty;
		
		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 9)]
		public int? OrderIndex { get; set; }

	}

}
