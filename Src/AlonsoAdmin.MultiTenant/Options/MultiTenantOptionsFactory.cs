using AlonsoAdmin.MultiTenant.Accessor;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Options
{
    internal class MultiTenantOptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly Action<TOptions, TenantInfo> _tenantConfig;
        private readonly IMultiTenantContextAccessor _multiTenantContextAccessor;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;

        public MultiTenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups, 
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, 
            Action<TOptions, TenantInfo> tenantConfig, 
            IMultiTenantContextAccessor multiTenantContextAccessor
            )
        {
            _setups = setups;
            _tenantConfig = tenantConfig;
            _multiTenantContextAccessor = multiTenantContextAccessor;
            _postConfigures = postConfigures;
        }

        public TOptions Create(string name)
        {
            var options = new TOptions();
            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else if (name == Microsoft.Extensions.Options.Options.DefaultName)
                {
                    setup.Configure(options);
                }
            }

            // 配置租户参数
            if (_multiTenantContextAccessor.MultiTenantContext?.TenantInfo != null)
            {
                _tenantConfig(options, _multiTenantContextAccessor.MultiTenantContext.TenantInfo);
            }

            foreach (var post in _postConfigures)
            {
                post.PostConfigure(name, options);
            }
            return options;
        }

    }
}
