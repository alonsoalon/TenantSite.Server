using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Exceptions
{
    /// <summary>
    /// 未登录异常
    /// </summary>
    public class UnLoginException : Exception
    {

        public UnLoginException()
        {}

        public UnLoginException(string message) : base(message)
        {}
    }
}
