using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Services.UPyun
{
    public interface IUPyunService
    {
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        string GetSignature(string policy);
        /// <summary>
        /// 加密签名计算
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string HmacSha1Sign(string text);
        /// <summary>
        /// 获取参数策略
        /// </summary>
        /// <param name="path"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        string GetPolicy(string path, string expiration);
        /// <summary>
        /// 获取时间戳  
        /// </summary>
        /// <returns></returns>
        string GetTimeStamp();
    }
}
