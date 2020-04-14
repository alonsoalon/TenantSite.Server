
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 数据字典分类
	/// </summary>
	[Table(Name = "sys_dictionary")]
	public class SysDictionaryEntity : BaseEntity
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
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 5)]
		public int? OrderIndex { get; set; }


		/// <summary>
		/// 扩展字段1
		/// </summary>
		[Column(Name = "EX1", Position = 6)]
		public string Ex1 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段2
		/// </summary>
		[Column(Name = "EX2", Position = 7)]
		public string Ex2 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段3
		/// </summary>
		[Column(Name = "EX3",Position = 8)]
		public string Ex3 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段4
		/// </summary>
		[Column(Name = "EX4", Position = 9)]
		public string Ex4 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段5
		/// </summary>
		[Column(Name = "EX5", Position = 10)]
		public string Ex5 { get; set; } = string.Empty;


		#region 导航属性
		[Navigate("Id")]
		public virtual ICollection<SysDictionaryDetailEntity> DictionaryItems { get; set; }
		#endregion

	}

}
