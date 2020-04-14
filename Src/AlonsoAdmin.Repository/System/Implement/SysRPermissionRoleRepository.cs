using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysRPermissionRoleRepository : RepositoryBase<SysRPermissionRoleEntity>, ISysRPermissionRoleRepository
    {

        public SysRPermissionRoleRepository(IMultiTenantDbFactory dbFactory, IAuthUser user): base(dbFactory.Db(Constants.Dbkey), user)
        {
         
        }

    }
}
