using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Mango.Framework.Infrastructure;
namespace Mango.Framework.Services.Tencent.Captcha
{
    public class TencentCaptcha:ITencentCaptcha
    {
        private CaptchaOptions _captchaOptions;
        public TencentCaptcha(CaptchaOptions captchaOptions)
        {
            _captchaOptions = captchaOptions;
        }
        /// <summary>
        /// 腾讯验证码服务端验证结果查询
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="randstr"></param>
        /// <param name="userIP"></param>
        /// <returns></returns>
        public bool QueryTencentCaptcha(string ticket, string randstr, string userIP)
        {
            string url = "https://ssl.captcha.qq.com/ticket/verify";
            string p = string.Format("aid={0}&AppSecretKey={1}&Ticket={2}&Randstr={3}&UserIP={4}", _captchaOptions.AppId, _captchaOptions.SecretKey, ticket, randstr, userIP);
            string httpResult = HttpHelper.Get(string.Format("{0}?{1}", url, p));
            TencentCaptchaResult tencentCaptchaResult = JsonConvert.DeserializeObject<TencentCaptchaResult>(httpResult);
            return tencentCaptchaResult.response == 1 ? true : false;
        }
    }
}
