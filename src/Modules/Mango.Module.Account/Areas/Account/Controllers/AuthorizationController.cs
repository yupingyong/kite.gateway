using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mango.Module.Core.Entity;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Mango.Framework.Services.Tencent.Captcha;
using Mango.Framework.Services.Aliyun.Sms;
using Mango.Framework.Services.EMail;

namespace Mango.Module.Account.Areas.Account.Controllers
{
    [Area("Account")]
    public class AuthorizationController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        private IMemoryCache _memoryCache;
        private ITencentCaptcha _tencentCaptcha;
        private IEMailService _emailService;
        public AuthorizationController(IUnitOfWork<MangoDbContext> unitOfWork
            , IMemoryCache memoryCache
            , ITencentCaptcha tencentCaptcha
            , IEMailService emailService)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _tencentCaptcha = tencentCaptcha;
            _emailService = emailService;
        }
        /// <summary>
        /// 邮箱验证码发送
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ticket"></param>
        /// <param name="randstr"></param>
        /// <returns></returns>
        public async Task<IActionResult> SendEmailValidateCode(string email, string ticket, string randstr)
        {
            string userIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            bool tencentCaptchaResult = _tencentCaptcha.QueryTencentCaptcha(ticket, randstr, userIP);
            if (!tencentCaptchaResult)
            {
                return APIReturnMethod.ReturnFailed("你的验证操作没有通过!");
            }
            Regex regex = new Regex(@"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");
            if (!regex.IsMatch(email))
            {
                return APIReturnMethod.ReturnFailed("请输入正确的电子邮箱!");
            }
            var repository = _unitOfWork.GetRepository<m_Account>();
            if (repository.Query().Where(q => q.AccountName == email).Count() > 0)
            {
                return APIReturnMethod.ReturnFailed("该电子邮箱已经注册,请勿重复注册!");
            }

            //邮件发送处理
            string emailCode = new Random().Next(101326, 985963).ToString();
            _memoryCache.Set<string>(email, emailCode);

            string messageContent = string.Format("感谢您的注册,您的验证码为:{0},您可以继续完成您的注册!", emailCode);
            string subject = "51Core技术网用户注册验证码邮件";

            bool sendResult = await _emailService.SendEmail(email, subject, messageContent);
            //邮件发送内容保存
            m_Sms model = new m_Sms();
            model.Contents = string.Format("邮箱验证码为:{0} 服务器返回结果:{1}", emailCode, sendResult);
            model.IsOk = sendResult;
            model.Phone = email;
            model.SendIP = userIP;
            model.SendTime = DateTime.Now;
            var smsRepository = _unitOfWork.GetRepository<m_Sms>();
            smsRepository.Insert(model);
            _unitOfWork.SaveChanges();
            if (sendResult)
            {
                return APIReturnMethod.ReturnSuccess("注册验证码发送成功,请注意查收!");
            }
            return APIReturnMethod.ReturnFailed("注册验证码发送失败,请稍后再试!");
        }
    }
}