using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.ResponseEntity
{
    public interface IResponseEntity
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// 状态码 
        /// </summary>
        int Code { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        string Message { get; }

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
