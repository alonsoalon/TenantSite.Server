using AlonsoAdmin.Common.IdGenerator;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    public abstract class BaseIdEntity
    {
		/// <summary>
		/// 主键
		/// </summary>
		[Column(Name = "ID", Position = 1, IsPrimary = true)]
		[Snowflake]
		public long Id { get; set; }
	}
}
