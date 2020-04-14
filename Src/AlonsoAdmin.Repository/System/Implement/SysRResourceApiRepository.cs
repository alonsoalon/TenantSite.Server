using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysRResourceApiRepository : RepositoryBase<SysRResourceApiEntity>, ISysRResourceApiRepository
    {
        public SysRResourceApiRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {

        }

    }
}
