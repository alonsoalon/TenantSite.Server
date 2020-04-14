using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant
{
    public class TenantInfo
    {
        /// <summary>
        /// 租户唯一ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 租户唯一Code，用着与路由匹配
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 租户数据库配置
        /// </summary>
        public IEnumerable<DbInfo> DbOptions { get; set; }

        /// <summary>
        /// 额外参数
        /// </summary>
        public IDictionary<string, object> Items { get; internal set; } = new Dictionary<string, object>();

        /// <summary>
        /// 租户上下文
        /// </summary>
        public MultiTenantContext MultiTenantContext { get; internal set; }

     
    }

    public class DbInfo
    {
        public string Key { get; set; }
        public string DbType { get; set; }
        public IEnumerable<DbConnectionString> ConnectionStrings { get; set; }
    }

    public class DbConnectionString
    {
        public string ConnectionString { get; set; }
        public DbUseType UseType { get; set; }
    }

    public enum DbUseType
    {
        Master,
        Slave
    }
}
