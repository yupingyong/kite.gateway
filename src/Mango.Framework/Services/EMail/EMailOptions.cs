using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Services.EMail
{
    public class EMailOptions
    {
        /// <summary>
        /// 邮件发送人名字
        /// </summary>
        public string FromName { get; set; }
        /// <summary>
        /// 发送人邮件
        /// </summary>
        public string FromEMail { get; set; }
        /// <summary>
        /// Smtp服务器地址
        /// </summary>
        public string SmtpServerUrl { get; set; }
        /// <summary>
        /// Smtp服务器端口号
        /// </summary>
        public int SmtpServerPort { get; set; }
        /// <summary>
        /// Smtp邮件服务器验证登录邮件
        /// </summary>
        public string SmtpAuthenticateEmail { get; set; }
        /// <summary>
        /// Smtp邮件服务器验证登录密码
        /// </summary>
        public string SmtpAuthenticatePasswordText { get; set; }
    }
}
