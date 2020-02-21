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
    public class EditController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public EditController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{area}/{controller}/{id}")]
        public IActionResult Index(int id)
        {
            Models.EditViewModel viewModel = new Models.EditViewModel();
            //获取频道数据
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
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
            //获取文章内容数据
            int accountId= HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
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
                .Where(q => q.StateCode == 1 && q.ContentsId == id && q.AccountId == accountId)
                .OrderByDescending(q => q.ContentsId)
                .FirstOrDefault();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Index(Models.ContentsEditRequestModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.Title) || requestModel.Title == "")
            {
                return APIReturnMethod.ReturnFailed("标题不能为空");
            }
            if (string.IsNullOrEmpty(requestModel.Contents) || requestModel.Contents == "")
            {
                return APIReturnMethod.ReturnFailed("内容不能为空");
            }
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            //
            Entity.m_CmsContents entity = repository.Query().Where(q => q.ContentsId == requestModel.ContentsId).FirstOrDefault();
            entity.Contents = requestModel.Contents;//Framework.Core.HtmlFilter.SanitizeHtml(model.Contents);
            entity.LastTime = DateTime.Now;
            entity.Title = requestModel.Title;
            entity.ContentsId = requestModel.ContentsId;
            entity.ChannelId = requestModel.ChannelId;

            repository.Update(entity);
            int resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
    }
}