using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services.Tenant
{
    public class TenantOtherOptions
    {

        /// <summary>
        /// 订阅者（用于说明该JWT发送给的用户）
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public string ExpirationMinutes { get; set; }

        /// <summary>
        /// 签发者（用于说明该JWT是由谁签发的）
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// token加密密匙
        /// </summary>
        public string Secret { get; set; }

    }

    //public class DbConnectionString
    //{
    //    public string ConnectionString { get; set; }
    //    public DbUseType UseType { get; set; }
    //}

    //public enum DbUseType
    //{
    //    Master,
    //    Slave
    //}

}
