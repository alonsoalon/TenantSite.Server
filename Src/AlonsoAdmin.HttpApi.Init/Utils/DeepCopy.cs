using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AlonsoAdmin.HttpApi.Init.Utils
{
    public static class DeepCopy
    {
        public static T DeepCopyByReflect<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        public static T DeepCopyByXml<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        public static T DeepCopyByBin<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// Copy Simple Property and Fileds
        /// 拷贝简单属性和公共字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyToAll<T>(this object source, T target) where T : class
        {
            if (source == null)
            {
                return;
            }

            if (target == null)
            {
                throw new ApplicationException("target 未实例化！");
            }

            var properties = target.GetType().GetProperties();
            foreach (var targetPro in properties)
            {
                try
                {
                   
                    var propertyValue = source.GetType().GetProperty(targetPro.Name).GetValue(source, null);
                    if (propertyValue != null)
                    {
                        if (propertyValue.GetType().IsEnum)
                        {
                            continue;
                        }

                        target.GetType().InvokeMember(targetPro.Name, BindingFlags.SetProperty, null, target, new object[] { propertyValue });
                    }

                    
                }
                catch (Exception ex)
                {
                    string strErr = ex.ToString();
                }
            }

        }


    }
}
