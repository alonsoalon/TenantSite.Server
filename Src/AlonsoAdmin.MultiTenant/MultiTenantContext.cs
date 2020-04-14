using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant
{
    public class MultiTenantContext
    {
        public TenantInfo TenantInfo { get; internal set; }
        public StrategyInfo StrategyInfo { get; internal set; }
        public StoreInfo StoreInfo { get; internal set; }
    }
}
