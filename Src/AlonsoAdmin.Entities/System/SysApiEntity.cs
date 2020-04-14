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
		/// URL
		/// </summary>
		[Column(Name = "URL", Position = 4)]
		public string Url { get; set; } = string.Empty;


		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRResourceApiEntity))]		
		public virtual ICollection<SysResourceEntity> Resource { get; set; }
		#endregion
	}

}
