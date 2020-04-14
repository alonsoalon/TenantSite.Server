using AlonsoAdmin.MultiTenant;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Repository
{
    public interface IMultiTenantDbFactory
    {
        public TenantInfo Tenant { get; }
        IFreeSql Db(string dbKey = "default");
    }
}
