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
    public class ChannelController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ChannelController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("{area}/{controller}")]
        [Route("{area}/{controller}/{p}")]
        public IActionResult Index([FromRoute]int p = 1)
        {
            var viewModel = LoadMainData(0, p);

            return View(viewModel);
        }
        [Route("{area}/{controller}/{action}/{id}")]
        [Route("{area}/{controller}/{action}/{id}/{p}")]
        public IActionResult List([FromRoute]int id = 0,[FromRoute]int p=1)
        {
            var viewModel = LoadMainData(id, p);
            return View("~/Areas/Cms/Views/Channel/Index.cshtml",viewModel);
        }
        public Models.ChannelViewModel LoadMainData(int id,int p)
        {
            Models.ChannelViewModel viewModel = new Models.ChannelViewModel();
            //获取频道数据
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();

            viewModel.ChannelListData = channelRepository.Query()
                .OrderBy(q => q.SortCount)
                .Select(q=>new Models.ChannelDataModel() { 
                    AppendTime=q.AppendTime.Value,
                    ChannelId=q.ChannelId.Value,
                    ChannelName=q.ChannelName,
                    RemarkText=q.RemarkText,
                    SortCount=q.SortCount.Value,
                    StateCode=q.StateCode.Value
                })
                .ToList();
            //获取帖子数据
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            viewModel.ContentsListData = repository.Query()
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
                .Where(q => q.StateCode == 1 && (id > 0 ? q.ChannelId == id : q.ChannelId != 0))
                .OrderByDescending(q => q.ContentsId)
               .Skip(10 * (p - 1))
               .Take(10)
               .ToList();
            return viewModel;
        }
    }
}