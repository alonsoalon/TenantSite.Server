using AlonsoAdmin.Common.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Configs
{
    public class StartupConfig
    {
        /// <summary>
        /// 缓存配置
        /// </summary>
        public CacheConfig Cache { get; set; }

        /// <summary>
        /// 操作日志配置参数
        /// </summary>
        public LogConfig Log { get; set; }

        /// <summary>
        /// 租户路由策略 0:Route 1:Host
        /// </summary>
        public TenantRouteStrategy TenantRouteStrategy { get; set; }

    }

    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType Type { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfig Redis { get; set; }
    }

    public class RedisConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }

    public class LogConfig
    {
        /// <summary>
        /// 操作日志开关
        /// </summary>
        public bool Operation { get; set; }
    }

    public enum TenantRouteStrategy
    {
        Route,
        Host,
    }
}
