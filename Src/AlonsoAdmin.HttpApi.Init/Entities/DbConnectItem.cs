using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Init.Entities
{
    public class DbConnectItem
    {
        public string DbType { get; set; }

        public string ConnectionString { get; set; }
    }

    public class DbCreateItem: DbConnectItem
    {
        public string CreateDbCommand { get; set; }
    }
}
