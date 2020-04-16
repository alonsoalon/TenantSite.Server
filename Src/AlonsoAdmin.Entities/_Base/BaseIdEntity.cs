
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities
{
	public abstract class BaseIdEntity : IBaseId
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Column(Name = "ID", Position = 1, IsPrimary = true)]
		[Snowflake]
		public string Id { get; set; }
	}
}
