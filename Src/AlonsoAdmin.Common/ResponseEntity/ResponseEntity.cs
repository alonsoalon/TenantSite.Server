using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.ResponseEntity
{
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

        public static IResponseEntity Ok(object p)
        {
            throw new NotImplementedException();
        }
    }
}
