using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Mango.Module.Core.Entity;
using Newtonsoft.Json;

namespace Mango.Module.CMS.Areas.CMS.Controllers
{
    [Area("CMS")]
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
        public IActionResult Article([FromRoute]int p = 1)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            Models.MyArticleViewModel viewModel = new Models.MyArticleViewModel();
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            viewModel.ListData = repository.Query()
                .Join(accountRepository.Query(), c => c.AccountId, account => account.AccountId, (c, account) => new { c, account })
                .Join(channelRepository.Query(), ca => ca.c.ChannelId, channel => channel.ChannelId, (ca, channel) => new Models.ContentsListDataModel()
                {
                    AccountId = ca.c.AccountId.Value,
                    AnswerCount = ca.c.AnswerCount.Value,
                    ChannelId = ca.c.ChannelId.Value,
                    ChannelName = channel.ChannelName,
                    ContentsId = ca.c.ContentsId.Value,
                    HeadUrl = ca.account.HeadUrl,
                    LastTime = ca.c.LastTime.Value,
                    NickName = ca.account.NickName,
                    PlusCount = ca.c.PlusCount.Value,
                    PostTime = ca.c.PostTime.Value,
                    ReadCount = ca.c.ReadCount.Value,
                    StateCode = ca.c.StateCode.Value,
                    Title = ca.c.Title
                })
                .Where(q => q.StateCode == 1 && q.AccountId == accountId)
                .OrderByDescending(q => q.ContentsId)
               .Skip(10 * (p - 1))
               .Take(10)
               .ToList();

            return View(viewModel);
        }
    }
}