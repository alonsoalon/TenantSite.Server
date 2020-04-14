using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Configs
{
    public class JwtConfig
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 有效期(分钟)
        /// </summary>
        public int ExpirationMinutes { get; set; }
    }
}
