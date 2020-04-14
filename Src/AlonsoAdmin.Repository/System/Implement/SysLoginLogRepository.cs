using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.MultiTenant;
using FreeSql;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public class SysLoginLogRepository : RepositoryBase<SysLoginLogEntity>, ISysLoginLogRepository
    {
        public SysLoginLogRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) 
            : base(dbFactory.Db(Constants.Dbkey), user)
        {
           
        }
    }
}
