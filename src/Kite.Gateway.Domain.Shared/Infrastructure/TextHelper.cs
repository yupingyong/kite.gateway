using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Kite.Gateway.Domain.Shared.Infrastructure
{
    public class TextHelper
    {
        /// <summary>  
        /// 该方法用于生成指定位数的随机数  
        /// </summary>  
        /// <param name="VcodeNum">参数是随机数的位数</param>  
        /// <returns>返回一个随机数字符串</returns>  
        public static string RndNum(int VcodeNum)
        {
            //验证码可以显示的字符集合  
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return code;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="SourceText">待加密原字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string SourceText)
        {
            string tempStr = "";
            MD5 md5 = MD5.Create();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(SourceText);//将字符编码为一个字节序列 
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
            md5.Dispose();

            for (int i = 0; i < md5data.Length; i++)
            {
                tempStr += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return tempStr.ToUpper();
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
        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SHA256EncryptString(string data)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            var hmacsha256 = SHA256.Create();
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashmessage.Length; i++)
            {
                builder.Append(hashmessage[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
