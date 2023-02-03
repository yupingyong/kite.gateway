using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Kite.Gateway.Application.Contracts.Dtos.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveAuthenticationDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        [Required]
        public bool UseState { get; set; }
        /// <summary>
        /// 颁发者
        /// </summary>
        [Required]
        public string Issuer { get; set; }
        /// <summary>
        /// 观察者
        /// </summary>
        [Required]
        public string Audience { get; set; }
        /// <summary>
        /// 时间偏移(秒)
        /// </summary>
        [Required]
        public int ClockSkew { get; set; }
        /// <summary>
        /// 是否验证签名
        /// </summary>
        [Required]
        public bool ValidateIssuerSigningKey { get; set; }
        /// <summary>
        /// 是否验证颁发者
        /// </summary>
        public bool ValidateIssuer { get; set; }
        /// <summary>
        /// 是否验证观察者
        /// </summary>
        [Required]
        public bool ValidateAudience { get; set; }
        /// <summary>
        /// 是否验证失效时间
        /// </summary>
        [Required]
        public bool ValidateLifetime { get; set; }
        /// <summary>
        /// 是否需要过期时间
        /// </summary>
        [Required]
        public bool RequireExpirationTime { get; set; }
        /// <summary>
        /// 是否启用SSL证书
        /// </summary>
        [Required]
        public bool UseSSL { get; set; }
        /// <summary>
        /// 秘钥字符串
        /// </summary>
        public string SecurityKeyStr { get; set; }
        /// <summary>
        /// 证书文件(BASE64:文件后缀名.pfx或者.cer 文件内容)
        /// </summary>
        public string CertificateFile { get; set; }
        /// <summary>
        /// 证书文件名(文件后缀名.pfx或者.cer 文件内容)
        /// </summary>
        public string CertificateFileName { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertificatePassword { get; set; }
    }
}
