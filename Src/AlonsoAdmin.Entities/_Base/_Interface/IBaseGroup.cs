using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseGroup
    {

		/// <summary>
		/// 数据归属组 为数据做数据权限提供方便
		/// </summary>
		public string GroupId { get; set; }
	}
}
