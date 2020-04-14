using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Entities
{
    /// <summary>
    /// 返回数据的通用接口
    /// 场景描述：
    /// 1.返回错误-当前段ajax请求返回响应状态码不等于200的时候，前端会进入Error回调函数，可由前端统一处理，此状态码Code将赋予响应码同样的值
    /// 2.返回错误-当前端ajax请求返回响应状态码等于200的时候，前端会进入Success回调函数，这时候分两中情况，
    ///     a:前端需要在具体业务模块中接管，并进行后续逻辑处理的情况，这种情况前端统一AJAX处理程序将放行响应到业务中
    ///     b:前端不需要在具体业务模块中接管，也就无后续逻辑的情况，这种情况前端统一AJAX处理程序将拦截响应并做统一的处理（如统一风格的提示）
    /// 3.返回成功 - 除开上面3中场景的所有情况 
    /// 
    /// 有了上面的场景描述，为了达到需求，执行规约如下：
    /// 场景1   : Success=false, Code = 错误响应码值（统一由错误中间件接管）
    /// 场景2.a : Success=false, Code = 业务定义的码值(除开0和1保留字外的值，这个码值将来可扩展用于前端错误消息多语言支持)
    /// 场景2.b : Success=false, Code = 0 此场景下前端将拦截响应，统一处理（一般可由前端统一处理的就几种错误，将来通过扩展多个Code值也可做到统一错误提示的多语言）
    /// 场景3   : Success=true，Code = 1
    /// 有了如上规约，前端就比较好做统一处理了，
    /// 进入Success回调中，只需要验证 Code == 0 就直接拦截下来 提示错误
    /// </summary>
    public interface IResponseEntity
    {
        /// <summary>
        /// 状态码 
        /// </summary>
        int Code { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 是否成功
        /// </summary>
        bool Success { get; }

    }

    /// <summary>
    /// 返回数据泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponseEntity<T> : IResponseEntity
    {
        /// <summary>
        /// 业务数据
        /// </summary>
        T Data { get; }
    }
}
