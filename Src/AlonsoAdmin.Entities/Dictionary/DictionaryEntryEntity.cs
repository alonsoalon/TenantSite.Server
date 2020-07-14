using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities.Dictionary
{
	/// <summary>
	/// 数据字典明细
	/// </summary>
	[Table(Name = "sys_dictionary_entry")]
	public class DictionaryEntryEntity : BaseEntity
	{

		/// <summary>
		/// 数据字典ID 
		/// </summary>
		[Column(Name = "DICTIONARY_HEADER_ID", Position = 2)]
		public string DictionaryHeaderId { get; set; }

		/// <summary>
		/// 上级ID
		/// </summary>
		[Column(Name = "PARENT_ID", Position = 3)]
		public string ParentId { get; set; } = string.Empty;

		/// <summary>
		/// 名称
		/// </summary>
		[Column(Name = "TITLE", Position = 4)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 数据值
		/// </summary>
		[Column(Name = "CODE", Position = 5)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 6)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 7)]
		[MaxValue]
		public int? OrderIndex { get; set; }

		/// <summary>
		/// 扩展字段1
		/// </summary>
		[Column(Name = "EX1", Position = 8)]
		public string Ex1 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段2
		/// </summary>
		[Column(Name = "EX2", Position = 9)]
		public string Ex2 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段3
		/// </summary>
		[Column(Name = "EX3", Position = 10)]
		public string Ex3 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段4
		/// </summary>
		[Column(Name = "EX4", Position = 11)]
		public string Ex4 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段5
		/// </summary>
		[Column(Name = "EX5", Position = 12)]
		public string Ex5 { get; set; } = string.Empty;

		#region 导航属性

		/// <summary>
		/// 父级
		/// </summary>
		public DictionaryEntryEntity Parent { get; set; }

		/// <summary>
		/// 子级集合
		/// </summary>
		public virtual ICollection<DictionaryEntryEntity> Childs { get; set; }

		/// <summary>
		/// 字典分类
		/// </summary>
		[Navigate("DictionaryHeaderId")]
		public virtual DictionaryHeaderEntity Dictionary { get; set; }
		#endregion

	}
}
