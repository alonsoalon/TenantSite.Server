using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public interface IMultiTenantStrategy
    {
        Task<string> GetIdentifierAsync(object context);
    }
}
