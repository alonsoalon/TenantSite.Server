using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.IdGenerator
{
    /// <summary>
    /// 弃用
    /// </summary>
    [Obsolete]
    public class Snowflake
    {
        /// <summary>
        /// 数据中心编号。取值为0-31
        /// </summary>
        private uint dataCenterId = 0;
        /// <summary>
        /// 机器编号。取值为0-31
        /// </summary>
        private uint workId = 0;
        //最后的时间戳
        private long lastTimestamp = -1L;
        //最后的序号
        private int lastIndex = -1;
        private Snowflake()
        { }
        static Snowflake snowflake;
        static object locker = new object();
        public static Snowflake Instance()
        {
            if (snowflake == null)
            {
                snowflake = new Snowflake();
            }
            return snowflake;
        }

        public void Init(uint dataCenterId, uint workId)
        {
            if (dataCenterId > 31)
            {
                throw new Exception("数据中心取值范围为0-31");
            }
            if (workId > 31)
            {
                throw new Exception("机器码取值范围为0-31");
            }
            this.dataCenterId = dataCenterId;
            this.workId = workId;
        }
        public long NextId()
        {
            lock (locker)
            {
                var currentTimeStamp = TimeStamp();
                if (currentTimeStamp < lastTimestamp)
                {
                    throw new Exception("时间戳生成出现错误");
                }
                if (currentTimeStamp == lastTimestamp)
                {
                    if (lastIndex < 4095)//为了保证长度
                    {
                        lastIndex++;
                    }
                    else
                    {
                        throw new Exception("单位毫秒内生成的id超过所支持的数量");
                    }
                }
                else
                {
                    lastIndex = 0;
                    lastTimestamp = currentTimeStamp;
                }
                var timeStr = Convert.ToString(currentTimeStamp, 2);
                var dcStr = Convert.ToString(dataCenterId, 2).PadLeft(5, '0');
                var wStr = Convert.ToString(workId, 2).PadLeft(5, '0'); ;
                var indexStr = Convert.ToString(lastIndex, 2).PadLeft(12, '0');
                return Convert.ToInt64($"0{timeStr}{dcStr}{wStr}{indexStr}", 2);
            }

        }
        public long TimeStamp()
        {
            var dt1970 = new DateTime(1970, 1, 1);
            return (DateTime.Now.Ticks - dt1970.Ticks) / 10000;
        }
    }
}
