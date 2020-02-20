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
        public IActionResult Message()
        {
            return View();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Article([FromRoute]int p=1)
        {
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            Models.MyArticleViewModel viewModel = new Models.MyArticleViewModel();
            //var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            //var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            //var accountRepository = _unitOfWork.GetRepository<m_Account>();
            //viewModel.ListData = repository.Query()
            //    .Join(accountRepository.Query(), c => c.AccountId, account => account.AccountId, (c, account) => new { c, account })
            //    .Join(channelRepository.Query(), ca => ca.c.ChannelId, channel => channel.ChannelId, (ca, channel) => new Models.ContentsListDataModel()
            //    {
            //        AccountId = ca.c.AccountId.Value,
            //        AnswerCount = ca.c.AnswerCount.Value,
            //        ChannelId = ca.c.ChannelId.Value,
            //        ChannelName = channel.ChannelName,
            //        ContentsId = ca.c.ContentsId.Value,
            //        HeadUrl = ca.account.HeadUrl,
            //        LastTime = ca.c.LastTime.Value,
            //        NickName = ca.account.NickName,
            //        PlusCount = ca.c.PlusCount.Value,
            //        PostTime = ca.c.PostTime.Value,
            //        ReadCount = ca.c.ReadCount.Value,
            //        StateCode = ca.c.StateCode.Value,
            //        Title = ca.c.Title
            //    })
            //    .Where(q => q.StateCode == 1 && q.AccountId == accountId)
            //    .OrderByDescending(q => q.ContentsId)
            //   .Skip(10 * (p - 1))
            //   .Take(10)
            //   .ToList();
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Theme([FromRoute]int p = 1)
        {
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            Models.MyThemeViewModel viewModel = new Models.MyThemeViewModel();
            //var apiResult = HttpCore.HttpGet($"/api/Docs/Theme/user/{accountId}/{p}");

            //if (apiResult.Code == 0)
            //{
            //    viewModel.ListData = JsonConvert.DeserializeObject<List<Models.ThemeDataModel>>(apiResult.Data.ToString());
            //}
            return View(viewModel);
        }
        [HttpGet("{area}/{controller}/{action}/{themeId}")]
        [HttpGet("{area}/{controller}/{action}/{themeId}/{p}")]
        public IActionResult Document([FromRoute]int themeId,[FromRoute]int p=1)
        {
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            Models.MyThemeDocumentViewModel viewModel = new Models.MyThemeDocumentViewModel();
            //var apiResult = HttpCore.HttpGet($"/api/Docs/Theme/user/{accountId}/{themeId}/{p}");

            //if (apiResult.Code == 0)
            //{
            //    viewModel.ListData = JsonConvert.DeserializeObject<List<Models.DocumentDataModel>>(apiResult.Data.ToString());
            //}
            return View(viewModel);
        }
    }
}