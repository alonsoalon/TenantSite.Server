
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlonsoAdmin.Services.Dictionary.Request
{
    /// <summary>
    /// 前端添加对象实体类 
    /// 命名规范：[业务名] + Add + Request
    /// </summary>
    public class DictionaryHeaderAddRequest
    {

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "Code不能为空！")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "标题不能为空！")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string Ex1 { get; set; } = string.Empty;

        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string Ex2 { get; set; } = string.Empty;

        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string Ex3 { get; set; } = string.Empty;

        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string Ex4 { get; set; } = string.Empty;

        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string Ex5 { get; set; } = string.Empty;

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// 数据归属组
        /// </summary>
        public string GroupId { get; set; } = string.Empty;
    }

    /// <summary>
    /// 前端编辑对象实体类
    /// 命名规范：[业务名] + Edit + Request
    /// 继承 '前端添加对象实体类'
    /// </summary>
    public class DictionaryHeaderEditRequest : DictionaryHeaderAddRequest
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderIndex { get; set; }

        /// <summary>
        /// 记录版本（乐观锁，用于限制脏写）
        /// </summary>
        public int Revision { get; set; }
    }

    /// <summary>
    /// 前端查询对象实体类
    /// 命名规范：[业务名] + Filter + Request
    /// </summary>
    public class DictionaryHeaderFilterRequest
    {

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 是否包含禁用的数据
        /// </summary>
        public bool WithDisable { get; set; } = false;



    }
}
