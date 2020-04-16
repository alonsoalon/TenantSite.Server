using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseRevision
    {
        /// <summary>
		/// 乐观锁 即是在数据记录版本号
		/// </summary>
        public int Revision { get; set; }
    }
}
