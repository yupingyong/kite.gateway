using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Framework.Services.Tencent.Captcha
{
    public interface ITencentCaptcha
    {
        bool QueryTencentCaptcha(string ticket, string randstr, string userIP);
    }
}
