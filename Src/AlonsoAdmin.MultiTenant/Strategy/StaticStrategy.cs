using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class StaticStrategy : IMultiTenantStrategy
    {
        internal readonly string identifier;

        public StaticStrategy(string identifier)
        {
            this.identifier = identifier;
        }

        public async Task<string> GetIdentifierAsync(object context)
        {
            return await Task.FromResult(identifier);
        }
    }
}
