using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseAdd
    {

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		/// 创建人名称
		/// </summary>
		public string CreatedByName { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreatedTime { get; set; }

		
	}
}
