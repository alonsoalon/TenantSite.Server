using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 后台接口
	/// </summary>
	[Table(Name = "sys_api")]
	public class SysApiEntity : BaseEntity
	{

		/// <summary>
		/// 分类
		/// </summary>
		[Column(Name = "CATEGORY", Position = 2)]
		public string Category { get; set; } = string.Empty;


		/// <summary>
		/// 标题
		/// </summary>
		[Column(Name = "TITLE", Position = 3)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 4, StringLength = 500)]
		public string Description { get; set; } = string.Empty;


		/// <summary>
		/// URL,API接口相对路径
		/// </summary>
		[Column(Name = "URL", Position = 5, StringLength = 500)]
		public string Url { get; set; } = string.Empty;

		/// <summary>
		/// 请求方法
		/// </summary>
		[Column(Name = "HTTP_METHOD", Position = 6, StringLength = 50)]
		public string HttpMethod { get; set; }

		/// <summary>
		/// 启用API验证，为false时，只要登录了系统即可访问API，为True时，将验证用户权限
		/// </summary>
		[Column(Name = "IS_VALIDATION", Position = 7)]
		public bool IsValidation { get; set; } = true;

		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 8)]
		[MaxValue]
		public int? OrderIndex { get; set; } = 0;


		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRResourceApiEntity))]		
		public virtual ICollection<SysResourceEntity> Resource { get; set; }
		#endregion
	}

}
