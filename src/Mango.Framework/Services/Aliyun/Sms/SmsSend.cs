using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Mango.Framework.Services.Aliyun.Sms
{
    public class SmsSend: IAliyunSmsSend
    {
        private AliyunOptions _aliyunOptions;
        private SmsOptions _smsOptions;
        public SmsSend(AliyunOptions aliyunOptions,SmsOptions smsOptions)
        {
            _aliyunOptions = aliyunOptions;
            _smsOptions = smsOptions;
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <returns></returns>
        public async Task<(bool success, string response)> SendSmsCode(string phone, string code)
        {
            try
            {
                IDictionary<string, string> data = new Dictionary<string, string>();
                data.Add("code", code);
                var sms = new SmsObject
                {
                    Mobile = phone,
                    Signature = _smsOptions.SmsSignature,
                    TempletKey = _smsOptions.SmsTempletKey,
                    Data = data,
                    OutId = "OutId"
                };

                return await new AliyunSms(_aliyunOptions.AccessKeyId, _aliyunOptions.AccessKeySecret).Send(sms);
                
            }
            catch
            {
                return (false, response: "");
            }
        }
    }
}
