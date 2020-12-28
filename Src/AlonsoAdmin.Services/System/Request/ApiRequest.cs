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
	public class ApiAddRequest
    {
	
		/// <summary>
		/// 标题
		/// </summary>
		[Required(ErrorMessage = "标题不能为空！")]
		public string Title { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// API分类
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// 接口地址
		/// </summary>
		[Required(ErrorMessage = "接口地址不能为空！")]
		public string Url { get; set; }

		/// <summary>
		/// Api接口方法
		/// </summary>
		public string HttpMethod { get; set; }

		/// <summary>
		/// 启用API验证，为false时，只要登录了系统即可访问API，为True时，将验证用户权限
		/// </summary>
		public bool IsValidation { get; set; }

		/// <summary>
		/// 数据归属组
		/// </summary>
		public string GroupId { get; set; } = string.Empty;

		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool IsDisabled { get; set; } = false;
	}

	public class ApiEditRequest : ApiAddRequest
	{
		public string Id { get; set; }

		public int? OrderIndex { get; set; }

		public int Revision { get; set; }

	}

	public class ApiFilterRequest
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
