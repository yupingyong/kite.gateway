using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Mango.Framework.Infrastructure;
namespace Mango.Framework.Services.UPyun
{
    public class UPyunService:IUPyunService
    {
        private UPyunOptions _options;
        public UPyunService(UPyunOptions options)
        {
            _options = options;
        }
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public string GetSignature(string policy)
        {
            string SignatureText = $"POST&/{_options.BucketName}&{policy}";
            return $"UPYUN {_options.BucketName}:{HmacSha1Sign(SignatureText)}";
        }
        /// <summary>
        /// 加密签名计算
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string HmacSha1Sign(string text)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(text);

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(TextHelper.MD5Encrypt(_options.BucketPassword)));
            return Convert.ToBase64String(hmac.ComputeHash(byteData));
        }
        /// <summary>
        /// 获取参数策略
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetPolicy(string path, string expiration)
        {
            //时间戳
            string parm = "{\"bucket\":\"" + _options.BucketName + "\",\"expiration\":\"" + expiration + "\",\"save-key\":\"" + path + "\"}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(parm));
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
