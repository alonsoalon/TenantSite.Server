using AlonsoAdmin.Entities.Dictionary;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Services.Dictionary.Response
{
    /// <summary>
    /// 响应前端列表展示的实体对象
    /// 命名规范：[业务名] + ForList + Response
    /// 可直接继承数据库实体
    /// 自定义属性的两种场景：
    /// 1.对于字段比较多，无需全部展示时，可以不继承(取名也尽量保持一致)
    /// 2.专用于前端的字段展示，和数据库属性不一致（需在Mapper.cs中做映射逻辑）
    /// </summary>
    public class DictionaryEntryForListResponse : DictionaryEntryEntity
    {

    }

    /// <summary>
    /// 响应前端详情展示的实体对象
    /// 命名规范：[业务名] + ForItem + Response
    /// 可直接继承数据库实体(示例为直接继承)
    /// 自定义属性的两种场景：
    /// 1.对于字段比较多，无需全部展示时，可以不继承(取名也尽量保持一致)
    /// 2.专用于前端的字段展示，和数据库属性不一致（需在Mapper.cs中做映射逻辑）
    /// </summary>
    public class DictionaryEntryForItemResponse : DictionaryEntryEntity
    {

    }
}
