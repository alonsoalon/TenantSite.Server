using AlonsoAdmin.MultiTenant.Accessor;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Options
{
    public class MultiTenantOptionsCache<TOptions> : IOptionsMonitorCache<TOptions> where TOptions : class
    {
        private readonly IMultiTenantContextAccessor _multiTenantContextAccessor;

        private readonly ConcurrentDictionary<string, IOptionsMonitorCache<TOptions>> _map = new ConcurrentDictionary<string, IOptionsMonitorCache<TOptions>>();
        

        public MultiTenantOptionsCache(IMultiTenantContextAccessor multiTenantContextAccessor)
        {
            _multiTenantContextAccessor = multiTenantContextAccessor ?? throw new ArgumentNullException(nameof(multiTenantContextAccessor));
        }

        /// <summary>
        /// 清除当前租户参数缓存
        /// </summary>
        public void Clear()
        {
            var tenantId = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id ?? "";
            var cache = _map.GetOrAdd(tenantId, new OptionsCache<TOptions>());
            cache.Clear();
        }
        /// <summary>
        /// 清除指定租户参数缓存
        /// </summary>
        public void Clear(string tenantId)
        {
            tenantId = tenantId ?? "";
            var cache = _map.GetOrAdd(tenantId, new OptionsCache<TOptions>());

            cache.Clear();
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAll()
        {
            foreach (var cache in _map.Values)
            {
                cache.Clear();
            }
        }

        public TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {
            if (createOptions == null)
            {
                throw new ArgumentNullException(nameof(createOptions));
            }

            name = name ?? Microsoft.Extensions.Options.Options.DefaultName;
            var tenantId = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id ?? "";
            var cache = _map.GetOrAdd(tenantId, new OptionsCache<TOptions>());

            return cache.GetOrAdd(name, createOptions);
        }

        public bool TryAdd(string name, TOptions options)
        {
            name = name ?? Microsoft.Extensions.Options.Options.DefaultName;
            var tenantId = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id ?? "";
            var cache = _map.GetOrAdd(tenantId, new OptionsCache<TOptions>());

            return cache.TryAdd(name, options);
        }

        public bool TryRemove(string name)
        {
            name = name ?? Microsoft.Extensions.Options.Options.DefaultName;
            var tenantId = _multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id ?? "";
            var cache = _map.GetOrAdd(tenantId, new OptionsCache<TOptions>());

            return cache.TryRemove(name);
        }

    }
}
