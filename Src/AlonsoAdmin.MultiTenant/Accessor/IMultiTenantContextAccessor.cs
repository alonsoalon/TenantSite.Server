using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant.Accessor
{
    public interface IMultiTenantContextAccessor
    {
        MultiTenantContext MultiTenantContext { get; }
    }
}
