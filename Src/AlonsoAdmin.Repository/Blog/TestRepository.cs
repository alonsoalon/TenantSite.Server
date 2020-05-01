using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.System;

namespace AlonsoAdmin.Repository.Blog
{
    public class TestRepository : RepositoryBase<SysApiEntity>,ITestRepository
    {
   

        public TestRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) : base(dbFactory.Db(Constants.BlogDbKey), user)
        {

        }


    }
}
