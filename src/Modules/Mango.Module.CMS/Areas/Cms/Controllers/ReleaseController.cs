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
    public class ReleaseController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ReleaseController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //获取频道数据
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();

            var resultData = channelRepository.Query()
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
            return View(resultData);
        }
        [HttpPost]
        public IActionResult Index(Models.ReleaseRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            if (string.IsNullOrEmpty(requestModel.Title) || requestModel.Title == "")
            {
                return APIReturnMethod.ReturnFailed("标题不能为空");
            }
            if (string.IsNullOrEmpty(requestModel.Contents) || requestModel.Contents == "")
            {
                return APIReturnMethod.ReturnFailed("内容不能为空");
            }
            //
            Entity.m_CmsContents entity = new Entity.m_CmsContents();
            entity.Contents = HtmlFilter.SanitizeHtml(requestModel.Contents);
            entity.ImgUrl = string.Empty;
            entity.StateCode = 1;
            entity.PostTime = DateTime.Now;
            entity.PlusCount = 0;
            entity.LastTime = DateTime.Now;
            entity.Tags = "";
            entity.ReadCount = 0;
            entity.Title = requestModel.Title;
            entity.AccountId = requestModel.AccountId;
            entity.AnswerCount = 0;
            entity.ChannelId = requestModel.ChannelId;
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            repository.Insert(entity);
            int resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
    }
}