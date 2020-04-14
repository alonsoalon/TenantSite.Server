
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Collections.Generic;
using AlonsoAdmin.Entities.System.Enums;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 资源
	/// </summary>
	[Table(Name = "sys_resource")]
	public class SysResourceEntity : BaseEntity
	{

		/// <summary>
		/// 资源CODE 资源CODE
		/// </summary>
		[Column(Name = "CODE", Position = 2)]
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 资源名称 资源名称
		/// </summary>
		[Column(Name = "TITLE", Position = 3)]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 资源描述
		/// </summary>
		[Column(Name = "DESCRIPTION", Position = 4)]
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 资源类型 1.分组 2.菜单 3.功能点（功能点包括 按钮，显示区域，具体由前端控制）
		/// </summary>
		[Column(Name = "RESOURCE_TYPE", Position = 5, MapType = typeof(int), CanUpdate = false)]
		public ResourceType ResourceType { get; set; }

		/// <summary>
		/// 父级ID
		/// </summary>
		[Column(Name = "PARENT_ID", Position = 6)]
		public long ParentId { get; set; }

		/// <summary>
		/// 如果为分组，是否默认展开（ResourceType为分组类型的独有属性）
		/// </summary>
		[Column(Name = "OPENED", Position = 7)]
		public bool? Opened { get; set; }

		/// <summary>
		///  SPA视图组件name或外面链接（ResourceType为菜单类型的独有属性）
		/// </summary>
		[Column(Name = "URL", Position = 8)]
		public string Url { get; set; } = string.Empty;

		/// <summary>
		/// SPA视图组件path
		/// </summary>
		[Column(Name = "PATH", Position = 9)]
		public string Path { get; set; }

		/// <summary>
		/// ICON
		/// </summary>
		[Column(Name = "ICON", Position = 10)]
		public string Icon { get; set; } = string.Empty;

		/// <summary>
		/// 排序
		/// </summary>
		[Column(Name = "ORDER_INDEX", Position = 11)]
		public int? OrderIndex { get; set; } = 0;

		/// <summary>
		/// 隐藏
		/// </summary>
		[Column(Name = "IS_HIDDEN", Position = 12)]
		public bool IsHidden { get; set; } = false;

		/// <summary>
		/// 是否可关闭 （ResourceType为菜单类型的独有属性）
		/// </summary>
		[Column(Name = "CLOSABLE", Position = 13)]
		public bool? Closable { get; set; }

		/// <summary>
		/// URL类型（ResourceType为菜单类型的独有属性） 1.SPA视图组件 2.外面链接
		/// </summary>
		[Column(Name = "LINK_TYPE", Position = 14, MapType = typeof(int))]
		public LinkType LinkType { get; set; }

		/// <summary>
		/// 外链打开方式（ResourceType为菜单类型且LinkType为外链类型的属性） 1.内部窗口打开 2.外部窗口打开
		/// </summary>
		[Column(Name = "OPEN_MODE", Position = 15, MapType = typeof(int))]
		public ExternalLinkOpenMode OpenMode { get; set; }
		

		#region 导航属性
		[Navigate(ManyToMany = typeof(SysRResourceApiEntity))]
		public virtual ICollection<SysApiEntity> Apis { get; set; }

		[Navigate(ManyToMany = typeof(SysRRoleResourceEntity))]
		public virtual ICollection<SysRoleEntity> Roles { get; set; }
		#endregion


	}

}
