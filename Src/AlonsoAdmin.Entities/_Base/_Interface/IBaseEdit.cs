using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseEdit
    {
		/// <summary>
		/// 更新人
		/// </summary>
		public string UpdatedBy { get; set; }

		/// <summary>
		/// 更新人名称
		/// </summary>
		public string UpdatedByName { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UpdatedTime { get; set; }

	}
}
