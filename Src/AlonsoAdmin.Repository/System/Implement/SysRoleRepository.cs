using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysRoleRepository : RepositoryBase<SysRoleEntity>, ISysRoleRepository
    {
        public SysRoleRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {

        }

    }
}
