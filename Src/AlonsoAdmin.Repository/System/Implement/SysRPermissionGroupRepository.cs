using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysRPermissionGroupRepository : RepositoryBase<SysRPermissionGroupEntity>, ISysRPermissionGroupRepository
    {
        public SysRPermissionGroupRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.SystemDbKey), user)
        {

        }

    }
}
