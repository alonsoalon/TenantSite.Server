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

        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue">转换错误时返回值</param>
        /// <returns></returns>
        public static int ToInt(this object thisValue, int errorValue = 0)
        {
            int reval;
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }

        /// <summary>
        /// 转long
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToLong(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0L;

            long.TryParse(s.ToString(), out long result);
            return result;
        }

        /// <summary>
        /// 转换为money
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue">转换错误返回值</param>
        /// <returns></returns>
        public static double ToMoney(this object thisValue, double errorValue = 0)
        {
            double reval;
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }

        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue">当转换错误返回的值</param>
        /// <returns></returns>
        public static string ToString(this object thisValue, string errorValue = "")
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return errorValue;
        }

        /// <summary>
        /// 转换成Double/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="digits">小数位数</param>
        /// <returns></returns>
        public static double ToDouble(this object s, int? digits = null)
        {
            if (s == null || s == DBNull.Value)
                return 0d;

            double.TryParse(s.ToString(), out double result);

            if (digits == null)
                return result;

            return Math.Round(result, digits.Value);
        }


        /// <summary>
        /// 转换为Decimal
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object thisValue, decimal errorValue = 0)
        {
            decimal reval;
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }

        /// <summary>
        /// 转换为日期,转换错误时返回DateTime.MinValue
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object thisValue)
        {
            DateTime reval = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                reval = Convert.ToDateTime(thisValue);
            }
            return reval;
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object thisValue, DateTime errorValue)
        {
            DateTime reval;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return errorValue;
        }

        /// <summary>
        /// 转换Bool
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ToBool(this object thisValue)
        {
            bool reval = false;
            if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }


    }
}
