
using AlonsoAdmin.MultiTenant.Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.MultiTenant
{


    public class StoreInfo
    {
        public Type StoreType { get; internal set; }
        public IMultiTenantStore Store { get; internal set; }
        public MultiTenantContext MultiTenantContext { get; internal set; }
    }
}
