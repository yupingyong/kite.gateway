using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Framework.Services.Tencent.Captcha
{
    internal class TencentCaptchaResult
    {
        /// <summary>
        /// 1:验证成功，0:验证失败，100:AppSecretKey参数校验错误[required]
        /// </summary>
        public int response { get; set; }
        /// <summary>
        /// [0,100]，恶意等级[optional]
        /// </summary>
        public int evil_level { get; set; }
        /// <summary>
        /// 验证错误信息[optional]
        /// </summary>
        public string err_msg { get; set; }
    }
}
