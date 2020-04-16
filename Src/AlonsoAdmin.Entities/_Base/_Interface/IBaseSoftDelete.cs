using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public interface IBaseSoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
