using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Infrastructure
{
   public sealed  class Base64
    {
       /// <summary>
       /// 解密字符串
       /// </summary>
       /// <param name="encode"></param>
       /// <param name="str"></param>
       /// <returns></returns>
       public static string DecodeBase64(Encoding encode,string str)
       {
           byte[] bytes = Convert.FromBase64String(str);
           try 
           {
               return encode.GetString(bytes);
           }
           catch 
           {
               return "";
           }
       }
       /// <summary>
       /// 解密字符串
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static string DecodeBase64(string str)
       {
           return DecodeBase64(Encoding.UTF8, str);
       }
       /// <summary>
       /// 加密字符串
       /// </summary>
       /// <param name="encode"></param>
       /// <param name="source"></param>
       /// <returns></returns>
       public static string EncodeBase64(Encoding encode, string str)
       {
           byte[] bytes = encode.GetBytes(str);
           try 
           {
               return Convert.ToBase64String(bytes);
           }
           catch 
           {
                return "";
           }
       }
       /// <summary>
       /// 加密字符串
       /// </summary>
       /// <param name="source"></param>
       /// <returns></returns>
       public static string EncodeBase64(string str)
       {
           return EncodeBase64(Encoding.UTF8, str);
       }
    }
}
