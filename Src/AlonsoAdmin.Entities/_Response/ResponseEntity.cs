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
    /// 场景2.a : Success=false, Code = 业务定义的码值(除开0和1保留字外的值，这个码值将来可扩展用于前端错误消息多语言支持)，默认已设置成通用的200
    /// 场景2.b : Success=false, Code = 0 此场景下前端将拦截响应，统一处理（一般可由前端统一处理的就几种错误，将来通过扩展多个Code值也可做到统一错误提示的多语言）
    /// 场景3   : Success=true，Code = 1
    /// 有了如上规约，前端就比较好做处理了，
    /// 进入Success回调中，验证 Code == 0 就直接拦截下来 提示错误
    /// </summary>
    public class ResponseEntity<T> : IResponseEntity<T>
    {
        public bool Success { get; private set; }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public T Data { get; private set; }

        public ResponseEntity<T> Ok(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
            Code = 1;

            return this;
        }

        public ResponseEntity<T> Error(string message, T data = default(T))
        {
            Success = false;
            Data = data;
            Message = message;
            Code = 200;

            return this;
        }

        public ResponseEntity<T> Error(string message, int code, T data = default(T))
        {
            Success = false;
            Data = data;
            Message = message;
            Code = code;

            return this;
        }

    }


    /// <summary>
    /// 返回响应的静态部分重载方法
    /// </summary>
    public static partial class ResponseEntity 
    {


        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IResponseEntity Ok<T>(T data = default(T), string msg = "")
        {
            return new ResponseEntity<T>().Ok(data, msg);
        }

        public static IResponseEntity Ok()
        {
            return Ok<string>();
        }

        public static IResponseEntity Error(string msg)
        {
            return new ResponseEntity<string>().Error(msg);
        }

        public static IResponseEntity Error<T>(string msg, T data = default(T))
        {
            return new ResponseEntity<T>().Error(msg, data);
        }

        public static IResponseEntity Error(string msg, int code)
        {
            return new ResponseEntity<string>().Error(msg, code);
        }

        public static IResponseEntity Error<T>(string msg, int code, T data = default(T))
        {
            return new ResponseEntity<T>().Error(msg, code, data);
        }


        public static IResponseEntity Result(bool success, string errMsg = "")
        {
            return success ? Ok() : Error(errMsg);
        }

    }


}

