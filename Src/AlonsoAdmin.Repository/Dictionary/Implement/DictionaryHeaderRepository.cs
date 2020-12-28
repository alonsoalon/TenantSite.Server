using AlonsoAdmin.Common.Auth;
using AlonsoAdmin.Entities.Dictionary;
using AlonsoAdmin.Repository.Dictionary.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Repository.Dictionary.Implement
{
    public class DictionaryHeaderRepository : RepositoryBase<DictionaryHeaderEntity>, IDictionaryHeaderRepository
    {
        public DictionaryHeaderRepository(IMultiTenantDbFactory dbFactory, IAuthUser user) 
            : base(dbFactory.Db(Constants.SystemDbKey), user) 
        {

        }
    }
}
