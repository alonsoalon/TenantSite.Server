
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Entities.System.Enums;
using AlonsoAdmin.Services.System.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlonsoAdmin.Services.System.Response
{
    


    /// <summary>
    /// ForList 实体对象（一般用于列表页展示数据用）
    /// </summary>
    public class UserForListResponse : UserEditRequest
    {
    

        /// <summary>
        /// 权限岗名称
        /// </summary>
        public string PermissionName { get; set; }

		/// <summary>
		/// 所属组织机构
		/// </summary>
		public string GroupName { get; set; }

		/// <summary>
		/// 头像
		/// </summary>
		public string Avatar { get; set; }


		#region 创建人相关

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		/// 创建人名称
		/// </summary>
		public string CreatedByName { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreatedTime { get; set; }

		#endregion 创建人相关

		#region 更新人相关

		/// <summary>
		/// 更新人
		/// </summary>
		public string UpdatedBy { get; set; }

		/// <summary>
		/// 更新人名称
		/// </summary>
		public string UpdatedByName { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UpdatedTime { get; set; }
		#endregion

	}

	/// <summary>
	/// ForIem 实体对象（一般用于明细页展示数据用）
	/// </summary>
	public class UserForItemResponse : UserForListResponse
    {

    }
}
