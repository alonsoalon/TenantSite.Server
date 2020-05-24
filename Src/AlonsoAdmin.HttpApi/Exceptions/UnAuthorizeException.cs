using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Exceptions
{
    /// <summary>
    /// 未授权异常
    /// </summary>
    public class UnAuthorizeException: Exception
    {
        public UnAuthorizeException()
        { }

        public UnAuthorizeException(string message) : base(message)
        { }
    }
}
