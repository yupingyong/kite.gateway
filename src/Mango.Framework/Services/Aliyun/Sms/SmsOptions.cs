using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Framework.Services.Aliyun.Sms
{
    public class SmsOptions
    {
        /// <summary>
        /// 短信签名
        /// </summary>
        public string SmsSignature { get; set; }
        /// <summary>
        /// 短信模板
        /// </summary>
        public string SmsTempletKey { get; set; }
    }
}
