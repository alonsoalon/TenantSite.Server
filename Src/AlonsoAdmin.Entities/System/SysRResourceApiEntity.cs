using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace AlonsoAdmin.Entities.System
{

	/// <summary>
	/// 资源与API关系表
	/// </summary>
	[Table(Name = "sys_r_resource_api")]
	[Index("uk_sys_r_resource_api_resourceid_apiid", "ResourceId,ApiId", true)]
	public class SysRResourceApiEntity : BaseIdEntity
	{
		/// <summary>
		/// 资源ID 资源ID
		/// </summary>
		[Column(Name = "RESOURCE_ID", Position = 2)]
		public long ResourceId { get; set; } 

		/// <summary>
		/// API-ID API ID
		/// </summary>
		[Column(Name = "API_ID", Position = 3)]
		public long ApiId { get; set; } 


		#region 导航属性

		/// <summary>
		/// 资源
		/// </summary>
		[Navigate("ResourceId")]
		public virtual SysResourceEntity Resource { get; set; }

		/// <summary>
		/// API
		/// </summary>
		[Navigate("ApiId")]
		public virtual SysApiEntity Api { get; set; }

		#endregion
	}

}
