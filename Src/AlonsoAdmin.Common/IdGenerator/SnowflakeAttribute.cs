using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.Common.IdGenerator
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SnowflakeAttribute : Attribute
    {
    }
}
