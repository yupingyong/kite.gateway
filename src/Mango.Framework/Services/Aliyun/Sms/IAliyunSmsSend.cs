using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Mango.Framework.Services.Aliyun.Sms
{
    public interface IAliyunSmsSend
    {
        Task<(bool success, string response)> SendSmsCode(string phone, string code);
    }
}
