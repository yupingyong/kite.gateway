using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;
namespace Mango.Framework.Infrastructure
{
    public static class Transform
    {
        /// <summary>
        /// 转换为Decimal型
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string inputText, decimal defaultValue)
        {
            Regex r = new Regex(@"^\d+(\.\d+)?$");

            if (!(!string.IsNullOrEmpty(inputText) && inputText.Trim() != ""))
            {
                return defaultValue;
            }
            if (!r.Match(inputText.Trim()).Success)
            {
                return defaultValue;
            }
            return decimal.Parse(inputText.Trim());
        }
        /// <summary>
        /// 转换为Int64型
        /// </summary>
        /// 
        /// <param name="inputText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int64 GetInt64(string inputText, Int64 defaultValue)
        {
            Regex r = new Regex("^[0-9]*$");

            if (!(!string.IsNullOrEmpty(inputText) && inputText.Trim() != ""))
            {
                return defaultValue;
            }
            if (!r.Match(inputText.Trim()).Success)
            {
                return defaultValue;
            }
            return Int64.Parse(inputText.Trim());
        }
        /// <summary>
        /// 转换为int型
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string inputText,int defaultValue)
        {
            Regex r = new Regex("^[0-9]*$");

            if (!(!string.IsNullOrEmpty(inputText) && inputText.Trim() != ""))
            {
                return defaultValue;
            }
            if (!r.Match(inputText.Trim()).Success)
            {
                return defaultValue;
            }
            return int.Parse(inputText.Trim());
        }

    }
}
