using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Services.Aliyun.Sms
{
    internal class SmsObject
    {
        /// <summary>
        /// 手机号
        /// </summary>
        internal string Mobile { set; get; }

        /// <summary>
        /// 签名
        /// </summary>
        internal string Signature { get; set; }

        /// <summary>
        /// 模板Key
        /// </summary>
        internal string TempletKey { set; get; }

        /// <summary>
        /// 短信数据
        /// </summary>
        internal IDictionary<string, string> Data { set; get; }

        /// <summary>
        /// 业务ID
        /// </summary>
        internal string OutId { set; get; }
    }
}
