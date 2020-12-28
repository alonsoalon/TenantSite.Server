﻿using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{
	/// <summary>
	/// 添加实体类 要么继承 数据库实体类，要么属性尽量取名一致(除非迁就前端)，避免automapper做对应映射处理
	/// </summary>
	public class GroupAddRequest
    {
		/// <summary>
		/// 数据组编码 数据组编码
		/// </summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 数据组标题 数据组标题
		/// </summary>
		[Required(ErrorMessage = "标题不能为空！")]
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// 数据组描述
		/// </summary>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// 父级ID
		/// </summary>
		public string ParentId { get; set; }

		/// <summary>
		/// 是否默认展开
		/// </summary>
		public bool? Opened { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool IsDisabled { get; set; } = false;

		/// <summary>
		/// 数据归属组
		/// </summary>
		public string GroupId { get; set; } = string.Empty;
	}

	public class GroupEditRequest : GroupAddRequest
	{
		public string Id { get; set; }

		public int? OrderIndex { get; set; }

		public int Revision { get; set; }

	}

	public class GroupFilterRequest
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
