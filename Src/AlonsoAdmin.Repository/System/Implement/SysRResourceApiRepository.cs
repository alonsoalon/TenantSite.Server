using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.Repository.System
{
    public class SysRResourceApiRepository : RepositoryBase<SysRResourceApiEntity>, ISysRResourceApiRepository
    {
        public SysRResourceApiRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.SystemDbKey), user)
        {

        }



    }
}
