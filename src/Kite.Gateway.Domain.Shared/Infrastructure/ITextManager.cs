using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Infrastructure
{
    public interface ITextManager
    {
        /// <summary>
        /// 文本脱敏处理
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task<string> DesensitizationAsync(string text);
        /// <summary>
        /// 手机号脱敏处理
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task<string> DesensitizationPhoneAsync(string text);
    }
}
