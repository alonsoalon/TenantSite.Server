using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Repository
{
    public class UowManager : UnitOfWorkManager
    {
        public UowManager(IMultiTenantDbFactory DbFactory, string dbKey) : base(DbFactory.Db(dbKey))
        {

        }
    }
}
