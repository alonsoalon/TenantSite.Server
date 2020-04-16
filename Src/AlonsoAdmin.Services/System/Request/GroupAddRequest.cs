using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.System.Request
{
    public class GroupAddRequest
    {
		/// <summary>
		/// 数据组编码 数据组编码
		/// </summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>
		/// 数据组标题 数据组标题
		/// </summary>
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
		/// 是否禁用
		/// </summary>
		public bool IsDisabled { get; set; }
	}
}
