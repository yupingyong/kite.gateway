using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Authorization
{
    public class JwtTokenOptions
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 观察者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 时间偏移(秒)
        /// </summary>
        public int ClockSkew { get; set; }
        /// <summary>
        /// 是否验证签名
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; }
        /// <summary>
        /// 是否验证颁发者
        /// </summary>
        public bool ValidateIssuer { get; set; }
        /// <summary>
        /// 是否验证观察者
        /// </summary>
        public bool ValidateAudience { get; set; }
        /// <summary>
        /// 是否验证失效时间
        /// </summary>
        public bool ValidateLifetime { get; set; }
        /// <summary>
        /// 是否需要扩展时间
        /// </summary>
        public bool RequireExpirationTime { get; set; }
        /// <summary>
        /// 是否启用SSL证书
        /// </summary>
        public bool UseSSL { get; set; }
        /// <summary>
        /// 秘钥字符串
        /// </summary>
        public string SecurityKeyStr { get; set; }
        /// <summary>
        /// 证书路径(文件后缀名.pfx或者.cer)
        /// </summary>
        public string CertificatePath { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertificatePassword { get; set; }
    }
}
