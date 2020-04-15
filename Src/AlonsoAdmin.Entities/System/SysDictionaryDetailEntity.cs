
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{
	/// <summary>
	/// 数据字典明细
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(Name = "sys_dictionary_detail")]
	public class SysDictionaryDetailEntity : BaseEntity
	{
		/// <summary>
		/// 编码 编码
		/// </summary>
		[Column(Name = "CODE", Position = 2)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 数据字典ID 
		/// </summary>
		[Column(Name = "DICTIONARY_ID", Position = 3)]
		public string DictionaryId { get; set; }

		/// <summary>
		/// 上级ID
		/// </summary>
		[Column(Name = "PARENT_ID", Position = 4)]
		public string ParentId { get; set; } = string.Empty;

		/// <summary>
		/// 名称
		/// </summary>
		[Column(Name = "TITLE", Position = 5)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 数据值
		/// </summary>
		[Column(Name = "DATA_VALUE", Position = 6)]
		public string DataValue { get; set; } = string.Empty;

		/// <summary>
		/// 描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 7)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 8)]
		[MaxValue]
		public int? OrderIndex { get; set; }

		/// <summary>
		/// 扩展字段1
		/// </summary>
		[Column(Name = "EX1", Position = 9)]
		public string Ex1 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段2
		/// </summary>
		[Column(Name = "EX2", Position=10)]
		public string Ex2 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段3
		/// </summary>
		[Column(Name = "EX3", Position = 11)]
		public string Ex3 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段4
		/// </summary>
		[Column(Name = "EX4", Position = 12)]
		public string Ex4 { get; set; } = string.Empty;

		/// <summary>
		/// 扩展字段5
		/// </summary>
		[Column(Name = "EX5", Position = 13)]
		public string Ex5 { get; set; } = string.Empty;

		#region 导航属性

		/// <summary>
		/// 父级
		/// </summary>
		public SysDictionaryDetailEntity Parent { get; set; }

		/// <summary>
		/// 子级集合
		/// </summary>
		public virtual ICollection<SysDictionaryDetailEntity> Childs { get; set; }

		/// <summary>
		/// 字典分类
		/// </summary>
		[Navigate("DictionaryId")]
		public virtual SysDictionaryEntity Dictionary { get; set; }
		#endregion

	}

}
