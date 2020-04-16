
using FreeSql.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace AlonsoAdmin.Entities
{
	/// <summary>
	/// 最全公共属性基类，业务数据实体建议继承此类
	/// 包括了ID、创建人相关、编辑人相关、软删除、禁用标记、乐观锁、数据归属组
	/// </summary>
	public abstract class BaseEntity :
		BaseIdEntity,// 主键 实现实体类
		IBaseId, 
		IBaseRevision, 
		IBaseDisable, 
		IBaseSoftDelete, 
		IBaseGroup, 
		IBaseAdd, 
		IBaseEdit
	{

		/// <summary>
		/// 乐观锁 即是在数据记录版本号
		/// </summary>
		[Column(Name = "REVISION", Position = -11, IsVersion = true)]
		public int Revision { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		[Column(Name = "IS_DISABLED", Position = -10)]
		public bool IsDisabled { get; set; } = false;

		/// <summary>
		/// 是否删除
		/// </summary>

		[Column(Name = "IS_DELETED", Position = -9)]
		public bool IsDeleted { get; set; } = false;

		/// <summary>
		/// 数据归属组 为数据做数据权限提供方便
		/// </summary>
		[Column(Name = "GROUP_ID", Position = -8)]
		public string GroupId { get; set; }


		#region 创建人相关

		/// <summary>
		/// 创建人
		/// </summary>
		[Column(Name = "CREATED_BY", Position = -7, CanUpdate = false)]
		public string CreatedBy { get; set; }

		/// <summary>
		/// 创建人名称
		/// </summary>
		[Column(Name = "CREATED_BY_NAME", Position = -6, CanUpdate = false), MaxLength(50)]
		public string CreatedByName { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Column(Name = "CREATED_TIME", Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local)]
		public DateTime? CreatedTime { get; set; }

		#endregion 创建人相关

		#region 更新人相关

		/// <summary>
		/// 更新人
		/// </summary>
		[Column(Name = "UPDATED_BY", Position = -4, CanInsert = false)]
		public string UpdatedBy { get; set; }

		/// <summary>
		/// 更新人名称
		/// </summary>
		[Column(Name = "UPDATED_BY_NAME", Position = -3, CanInsert = false)]
		public string UpdatedByName { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		[Column(Name = "UPDATED_TIME", Position = -2, CanInsert = false, ServerTime = DateTimeKind.Local)]


		public DateTime? UpdatedTime { get; set; }

		#endregion 更新人相关


	}
}
