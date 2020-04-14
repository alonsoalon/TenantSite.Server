
using AlonsoAdmin.MultiTenant.Strategy;
using System;


namespace AlonsoAdmin.MultiTenant
{
    public class StrategyInfo
    {
        public Type StrategyType { get; internal set; }
        public IMultiTenantStrategy Strategy { get; internal set; }
        public MultiTenantContext MultiTenantContext { get; internal set; }
    }
}
