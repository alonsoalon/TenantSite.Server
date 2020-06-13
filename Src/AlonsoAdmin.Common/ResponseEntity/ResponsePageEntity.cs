using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.ResponseEntity
{
    public class ResponsePageEntity<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; } = 0;

        /// <summary>
        /// 数据列表
        /// </summary>
        public IList<T> List { get; set; }
    }
}
