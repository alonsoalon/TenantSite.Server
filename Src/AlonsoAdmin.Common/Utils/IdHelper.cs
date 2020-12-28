using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Utils
{
    public class IdHelper
    {

        public static long GenSnowflakeId(uint dataCenterId = 5, uint workId =20)
        {
            var idWorker = IdGenerator.Snowflake.Instance();
            idWorker.Init(dataCenterId, workId);
            var id = idWorker.NextId();
            return id;
        }
    }
}
