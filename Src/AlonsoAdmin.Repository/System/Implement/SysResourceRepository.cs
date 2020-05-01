
using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public class SysResourceRepository : RepositoryBase<SysResourceEntity>, ISysResourceRepository
    {
        public SysResourceRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.SystemDbKey), user)
        {
        
        }
    }
}
