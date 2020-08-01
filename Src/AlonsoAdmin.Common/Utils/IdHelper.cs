using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Utils
{
    public class IdHelper
    {

        public static long GenSnowflakeId(uint dataCenterId = 5, uint workId =20)
        {
            var sf = new Common.IdGenerator.SnowflakeId(dataCenterId, workId);
            var id = sf.NextId();
            return id;
        }
    }
}
