using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysRRoleResourceRepository : RepositoryBase<SysRRoleResourceEntity>, ISysRRoleResourceRepository
    {
        public SysRRoleResourceRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {

        }

    }
}
