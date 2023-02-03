using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Kite.Gateway.Domain.Shared.Text
{
    public class TextHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="SourceText">待加密原字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string SourceText)
        {
            string tempStr = "";
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(SourceText);//将字符编码为一个字节序列 
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
        /// <param name="Source_String"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        /// <summary>
        /// HmacSHA256加密
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string HmacSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
