using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;

namespace AlonsoAdmin.Repository.Blog
{
    public class TestRepository : RepositoryBase<SysApiEntity>,ITestRepository
    {
        //private IMultiTenantDbFactory _dbFactory;
        //private IAuthUser _user;
        //private string _dbKey;

        public TestRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.Dbkey), user)
        {
            //_dbFactory = dbFactory;
            //_dbKey = dbKey;
            //_user = user;
        }


    }
}
