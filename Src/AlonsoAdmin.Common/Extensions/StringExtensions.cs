using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AlonsoAdmin.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 判断字符串是否不为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }


        

        /// <summary>
        /// 计算MD5值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>字符串</returns>
        public static string Md5(this string text)
        {
            String md5 = "";
            MD5 md5_text = MD5.Create();
            byte[] temp = md5_text.ComputeHash(System.Text.Encoding.ASCII.GetBytes(text)); //计算MD5 Hash 值

            for (int i = 0; i < temp.Length; i++)
            {
                md5 += temp[i].ToString("x2"); //转码 每两位转一次16进制
            }
            return md5;
        }
    }
}
