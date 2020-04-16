using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseDisable
    {
        /// <summary>
		/// 是否禁用
		/// </summary>
        public bool IsDisabled { get; set; }
    }
}
