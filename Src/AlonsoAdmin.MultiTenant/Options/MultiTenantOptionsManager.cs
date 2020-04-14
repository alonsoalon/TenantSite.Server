using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Options
{
    internal class MultiTenantOptionsManager<TOptions> : IOptions<TOptions>, IOptionsSnapshot<TOptions> where TOptions : class, new()
    {
        private readonly IOptionsFactory<TOptions> _factory;
        private readonly IOptionsMonitorCache<TOptions> _cache; // Note: this is a private cache


        /// <summary>
        /// 使用指定的选项配置初始化新实例
        /// </summary>
        /// <param name="factory">创建的工厂</param>
        /// <param name="cache">参数</param>
        public MultiTenantOptionsManager(IOptionsFactory<TOptions> factory, IOptionsMonitorCache<TOptions> cache)
        {
            _factory = factory;
            _cache = cache;
        }

        public TOptions Value
        {
            get
            {
                return Get(Microsoft.Extensions.Options.Options.DefaultName);
            }
        }

        public virtual TOptions Get(string name)
        {
            name = name ?? Microsoft.Extensions.Options.Options.DefaultName;

            // Store the options in our instance cache.
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }

        public void Reset()
        {
            _cache.Clear();
        }
    }
}
