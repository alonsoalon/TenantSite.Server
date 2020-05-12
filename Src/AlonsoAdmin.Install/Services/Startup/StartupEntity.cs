using AlonsoAdmin.Common.Cache;
using AlonsoAdmin.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Install.Services.Startup
{




    public class StartupEntity
    {
        public CacheType CacheType { get; set; }

        public string RedisConnectionString { get; set; }

        public bool OperationlogOpen { get; set; }

        public TenantRouteStrategy TenantRouteStrategy { get; set; }

    }


}
