using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Kite.Gateway.Domain.Shared.Infrastructure
{
    internal class TextManager : ITextManager, ISingletonDependency
    {
        public Task<string> DesensitizationAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Task.FromResult("");
            }
            if (text.Length <= 4)
            {
                return Task.FromResult(text);
            }
            var chars = text.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (i > 3)
                {
                    chars[i] = '*';
                }
            }
            return Task.FromResult(string.Join('#', chars).Replace("#", ""));
        }

        public Task<string> DesensitizationPhoneAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Task.FromResult("");
            }
            var chars = text.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                if (i > 2 && i < 7)
                {
                    chars[i] = '*';
                }
            }
            return Task.FromResult(string.Join('#', chars).Replace("#", ""));
        }
    }
}
