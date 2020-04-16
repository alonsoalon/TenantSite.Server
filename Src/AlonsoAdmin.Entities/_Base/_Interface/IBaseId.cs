using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseId
    {
		/// <summary>
		/// 主键
		/// </summary>
		public string Id { get; set; }
	}
}
