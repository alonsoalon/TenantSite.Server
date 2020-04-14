using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Configs
{
    public class SystemConfig
    {
        /// <summary>
        /// 监听Curd操作
        /// </summary>
        public bool WatchCurd { get; set; }

        /// <summary>
        /// 用于雪花算法ID生成 - 数据中心取值范围为0-31
        /// </summary>
        public uint DataCenterId { get; set; }

        /// <summary>
        /// 用于雪花算法ID生成 - 机器码取值范围为0-31
        /// </summary>
        public uint WorkId { get; set; }


}
}
