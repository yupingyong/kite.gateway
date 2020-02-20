using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Mango.Framework.Infrastructure
{
    public class TextHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="sourceText">待加密原字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string sourceText)
        {
            string tempStr = "";
            MD5 md5 = MD5.Create();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(sourceText);//将字符编码为一个字节序列 
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
            md5.Dispose();

            for (int i = 0; i < md5data.Length; i++)
            {
                tempStr += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return tempStr.ToLower();
        }
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="sourceText"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string sourceText)
        {
            byte[] StrRes = Encoding.Default.GetBytes(sourceText);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider(); 
             StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

    }
}
