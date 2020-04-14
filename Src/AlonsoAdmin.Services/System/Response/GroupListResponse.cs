using AlonsoAdmin.Common.JsonConvert;
using AlonsoAdmin.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlonsoAdmin.Services.System.Response
{
    public class GroupListResponse : SysGroupEntity
    {
        [JsonConverter(typeof(IdToStringConverter))]

        /// <summary>
        /// 主键Id
        /// </summary>
        public new long Id { get; set; }
    }
}
