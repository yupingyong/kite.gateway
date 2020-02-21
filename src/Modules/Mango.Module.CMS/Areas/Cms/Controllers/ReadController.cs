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
    [Area("Cms")]
    public class ReadController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ReadController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("cms/read/{id}")]
        [HttpGet]
        public IActionResult Index(int id)
        {
            Models.ReadViewModel viewModel = new Models.ReadViewModel();
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            //获取帖子详情数据
            viewModel.ContentsData = repository.Query()
                .Join(accountRepository.Query(), c => c.AccountId, account => account.AccountId, (c, account) => new { c, account })
                .Join(channelRepository.Query(), ca => ca.c.ChannelId, channel => channel.ChannelId, (ca, channel) => new Models.ContentsDataModel()
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
                    Title = ca.c.Title,
                    Contents = ca.c.Contents
                })
                .Where(q => q.StateCode == 1 && q.ContentsId == id)
                .OrderByDescending(q => q.ContentsId)
                .FirstOrDefault();
            //获取热门帖子数据
            viewModel.HotListData = repository.Query()
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
                .Where(q => q.StateCode == 1).OrderByDescending(q => q.ReadCount).Where(q => q.PostTime >= DateTime.Now.AddDays(-7)).Take(10).ToList();
            //获取频道数据
            viewModel.ChannelListData = channelRepository.Query()
                .OrderBy(q => q.SortCount)
                .Select(q => new Models.ChannelDataModel()
                {
                    AppendTime = q.AppendTime.Value,
                    ChannelId = q.ChannelId.Value,
                    ChannelName = q.ChannelName,
                    RemarkText = q.RemarkText,
                    SortCount = q.SortCount.Value,
                    StateCode = q.StateCode.Value
                })
                .ToList();
            return View(viewModel);
        }
    }
}