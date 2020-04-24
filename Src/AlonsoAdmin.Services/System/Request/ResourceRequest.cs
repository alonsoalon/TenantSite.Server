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
    public class ResourceAddRequest
    {

		/// <summary>
		/// 资源CODE
		/// </summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 资源名称 资源名称
		/// </summary>、
		[Required(ErrorMessage = "标题不能为空！")]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 资源描述
		/// </summary>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 资源类型 1.分组 2.菜单 3.功能点（功能点包括 按钮，显示区域，具体由前端控制）
		/// </summary>
		public ResourceType ResourceType { get; set; }

		/// <summary>
		/// 父级ID
		/// </summary>
		public string ParentId { get; set; } = string.Empty;

		/// <summary>
		///  SPA视图组件name或外面链接（ResourceType为菜单类型的独有属性）
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// SPA视图组件path
		/// </summary>
		public string ViewPath { get; set; }
		/// <summary>
		/// SPA视图组件path
		/// </summary>
		public string ViewName { get; set; }

		/// <summary>
		/// SPA视图组件 是否缓存
		/// </summary>
		public bool ViewCache { get; set; }

		/// <summary>
		/// ICON
		/// </summary>
		public string Icon { get; set; } = string.Empty;


		/// <summary>
		/// 隐藏
		/// </summary>
		public bool IsHidden { get; set; } = false;

		/// <summary>
		/// 是否可关闭 （ResourceType为菜单类型的独有属性）
		/// </summary>
		public bool Closable { get; set; } = true;

		/// <summary>
		/// URL类型（ResourceType为菜单类型的独有属性） 1.SPA视图组件 2.外面链接
		/// </summary>
		public LinkType LinkType { get; set; }

		/// <summary>
		/// 外链打开方式（ResourceType为菜单类型且LinkType为外链类型的属性） 1.内部窗口打开 2.外部窗口打开
		/// </summary>
		public ExternalLinkOpenMode OpenMode { get; set; }

		/// <summary>
		/// 如果为分组，是否默认展开（ResourceType为分组类型的独有属性）
		/// </summary>
		public bool? Opened { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool IsDisabled { get; set; }


		/// <summary>
		/// 数据归属组
		/// </summary>
		public string GroupId { get; set; }


	}

	public class ResourceEditRequest : ResourceAddRequest
	{
		public string Id { get; set; }

		public int? OrderIndex { get; set; }

		public int Revision { get; set; }



	}

	public class ResourceFilterRequest
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
}
