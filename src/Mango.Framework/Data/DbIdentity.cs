using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
namespace Mango.Framework.Data
{
    public class DbIdentity
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static object Change(object value, Type type)
        {
            if (type.IsGenericParameter)
            {
                if (type.IsGenericParameter && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                    {
                        return null;
                    }
                    NullableConverter nullableConverter = new NullableConverter(type);
                    value = Convert.ChangeType(value, type);
                }
            }
            return value;
        }
    }
}
