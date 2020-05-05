using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Repository.System.Interface;

namespace AlonsoAdmin.Repository.System.Implement
{
    public class SysRPermissionRoleRepository : RepositoryBase<SysRPermissionRoleEntity>, ISysRPermissionRoleRepository
    {

        public SysRPermissionRoleRepository(IMultiTenantDbFactory dbFactory, IAuthUser user): base(dbFactory.Db(Constants.SystemDbKey), user)
        {
         
        }

    }
}
