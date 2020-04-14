using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysResourceRepository : RepositoryBase<SysResourceEntity>, ISysResourceRepository
    {
        public SysResourceRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {

        }

    }
}
