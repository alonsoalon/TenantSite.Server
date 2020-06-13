using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.ResponseEntity
{
    public class ResponseOptionEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public object Data { get; set; }
    }
}
