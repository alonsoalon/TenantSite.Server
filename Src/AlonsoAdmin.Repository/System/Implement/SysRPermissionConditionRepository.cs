using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysRPermissionConditionRepository : RepositoryBase<SysRPermissionConditionEntity>, ISysRPermissionConditionRepository
    {
        public SysRPermissionConditionRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {

        }

    }
}
