using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlonsoAdmin.MultiTenant.Strategy
{
    public class DelegateStrategy : IMultiTenantStrategy
    {
        private readonly Func<object, Task<string>> doStrategy;

        public DelegateStrategy(Func<object, Task<string>> doStrategy)
        {
            this.doStrategy = doStrategy ?? throw new ArgumentNullException(nameof(doStrategy));
        }

        public async Task<string> GetIdentifierAsync(object context)
        {
            var identifier = await doStrategy(context);
            return await Task.FromResult(identifier);
        }
    }
}
