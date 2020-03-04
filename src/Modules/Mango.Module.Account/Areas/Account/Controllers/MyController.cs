using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Module.Core.Entity;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Newtonsoft.Json;
namespace Mango.Module.Account.Areas.Account.Controllers
{
    [Area("Account")]
    public class MyController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public MyController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("{area}/{controller}/{action}")]
        [Route("{area}/{controller}/{action}/{p}")]
        public IActionResult Message([FromRoute]int p = 1)
        {
            var accountId= HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var repository = _unitOfWork.GetRepository<m_Message>();
            var accountRepository= _unitOfWork.GetRepository<m_Account>();

            Models.MyMessageViewModel viewModel = new Models.MyMessageViewModel();
            viewModel.ListData= repository.Query()
                .Join(accountRepository.Query(), msg => msg.AccountId, acc =>acc.AccountId, (msg,acc) => new Models.MessageModel()
                {
                    AppendAccountId=msg.AppendAccountId.Value,
                    Contents = msg.Contents,
                    IsRead = msg.IsRead.Value,
                    MessageId = msg.MessageId.Value,
                    MessageType = msg.MessageType.Value,
                    ObjectId = msg.ObjectId.Value,
                    PostTime = msg.PostTime.Value,
                    AccountId = msg.AccountId.Value,
                    HeadUrl = acc.HeadUrl,
                    NickName = acc.NickName
                })
                .Where(q=>q.AppendAccountId==accountId)
                .OrderByDescending(q => q.MessageId)
                .Skip(10 * (p - 1))
                .Take(10)
                .ToList();
            viewModel.TotalCount = repository.Query().Where(q => q.AppendAccountId == accountId).Select(q => q.MessageId).Count();
            return View(viewModel);
        }
        /// <summary>
        /// 账号信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Info()
        {
            Models.MyAccountViewModel viewModel = new Models.MyAccountViewModel();
            viewModel.AccountData=JsonConvert.DeserializeObject<Models.AccountDataModel>(HttpContext.Session.GetString("AccountLoginData"));
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Information(Models.InformationUpdateRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            if (string.IsNullOrEmpty(requestModel.InformationValue))
            {
                return APIReturnMethod.ReturnFailed("请输入待修改项的值!");
            }
            var repository = _unitOfWork.GetRepository<m_Account>();
            var accountData = repository.Query().Where(q => q.AccountId == requestModel.AccountId).FirstOrDefault();
            switch (requestModel.InformationType)
            {
                case 3:
                    accountData.Tags = requestModel.InformationValue;
                    break;
                case 4:
                    accountData.AddressInfo = requestModel.InformationValue;
                    break;
                case 5:
                    accountData.Sex = requestModel.InformationValue;
                    break;
                case 1:
                    accountData.NickName = requestModel.InformationValue;
                    break;
                case 2:
                    accountData.HeadUrl = requestModel.InformationValue;
                    break;
            }
            repository.Update(accountData);
            var resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        [HttpPost]
        public IActionResult Password(Models.PasswordUpdateRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            if (string.IsNullOrEmpty(requestModel.Password))
            {
                return APIReturnMethod.ReturnFailed("请输入您的原登录密码!");
            }
            if (string.IsNullOrEmpty(requestModel.NewPassword))
            {
                return APIReturnMethod.ReturnFailed("请输入您的新登录密码!");
            }
            var repository = _unitOfWork.GetRepository<m_Account>();
            var accountData = repository.Query().Where(q => q.AccountId == requestModel.AccountId && q.Password == TextHelper.MD5Encrypt(requestModel.Password)).FirstOrDefault();
            if (accountData == null)
            {
                return APIReturnMethod.ReturnFailed("请输入正确的原登录密码!");
            }
            accountData.Password = TextHelper.MD5Encrypt(requestModel.NewPassword);
            repository.Update(accountData);
            var resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
    }
}