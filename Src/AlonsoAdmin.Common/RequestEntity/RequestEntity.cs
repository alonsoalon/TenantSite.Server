using FreeSql.Internal.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.RequestEntity
{
    public class RequestEntity<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 50;

        /// <summary>
        /// 查询条件
        /// </summary>
        public T Filter { get; set; }

        /// <summary>
        /// 高级查询条件
        /// </summary>
        public DynamicFilterInfo DynamicFilter { get; set; } = null;
    }
}
