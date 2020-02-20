using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mango.Module.Core.Entity;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;

namespace Mango.Module.Account.Areas.Account.Controllers
{
    [Area("Account")]
    public class RegisterController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        private IMemoryCache _memoryCache;
        public RegisterController(IUnitOfWork<MangoDbContext> unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Models.AccountRegisterRequestModel requestModel)
        {
            string Result = string.Empty;
            var repository = _unitOfWork.GetRepository<m_Account>();
            var codeCache = _memoryCache.Get<string>(requestModel.AccountName);
            if (codeCache == null)
            {
                return APIReturnMethod.ReturnFailed("该账号与通过验证的账号不一致");
            }
            if (requestModel.ValidateCode != codeCache)
            {
                return APIReturnMethod.ReturnFailed("请输入正确的注册验证码!");
            }

            if (repository.Query().Where(q => q.AccountName == requestModel.AccountName).Count() > 0)
            {
                return APIReturnMethod.ReturnFailed("该账号已经注册过!");
            }
            //注册新用户
            m_Account entity = new m_Account();
            entity.HeadUrl = "/images/avatar.png";
            entity.GroupId = 1;
            entity.StateCode = 1;
            entity.LastLoginDate = DateTime.Now;
            entity.NickName = requestModel.NickName;
            entity.Password = TextHelper.MD5Encrypt(requestModel.Password);
            entity.Phone = "";
            entity.RegisterDate = DateTime.Now;
            entity.AccountName = requestModel.AccountName;
            entity.Email = "";
            entity.AddressInfo = "";
            entity.Birthday = "";
            entity.Sex = "男";
            entity.Tags = "";
            repository.Insert(entity);
            var resultCount = _unitOfWork.SaveChanges();
            if (resultCount > 0)
            {
                return APIReturnMethod.ReturnSuccess("恭喜您,您的账户已经注册成功!");
            }
            return APIReturnMethod.ReturnFailed("抱歉,您的注册失败,请稍后再尝试!");
        }
    }
}