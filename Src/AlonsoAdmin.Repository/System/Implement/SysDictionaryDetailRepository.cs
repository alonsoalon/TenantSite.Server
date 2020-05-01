using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;
using FreeSql;


namespace AlonsoAdmin.Repository.System
{
    public class SysDictionaryDetailRepository : RepositoryBase<SysDictionaryDetailEntity>, ISysDictionaryDetailRepository
    {
        public SysDictionaryDetailRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.SystemDbKey), user)
        {

        }

    }
}
