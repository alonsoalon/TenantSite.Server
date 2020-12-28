using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Tasks
{
    public class DbOption
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        
        /// <summary>
        /// 数据库类型
        /// </summary>
        public FreeSql.DataType DbType { get; set; }
    }

}
