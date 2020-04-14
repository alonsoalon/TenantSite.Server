using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对象转JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>字符串</returns>
        public static string SerializeToString(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

    }
}
