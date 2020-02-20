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

namespace Mango.Module.CMS.Controllers
{
    [Area("CMS")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ContentsController : ControllerBase
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ContentsController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 根据ID获取内容
        /// </summary>
        /// <param name="accountId">用户账号ID</param>
        /// <param name="contentsId">内容ID</param>
        /// <returns></returns>
        [HttpGet("user/{accountId}/{contentsId}")]
        public IActionResult Get([FromRoute]int accountId, [FromRoute] int contentsId)
        {
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var resultData = repository.Query()
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
                .Where(q => q.StateCode == 1 && q.ContentsId == contentsId&&q.AccountId== accountId)
                .OrderByDescending(q => q.ContentsId)
                .FirstOrDefault();
            return APIReturnMethod.ReturnSuccess(resultData);
        }
        /// <summary>
        /// 根据用户账号ID获取文章列表
        /// </summary>
        /// <param name="accountId">用户账号ID</param>
        /// <param name="p">页码</param>
        /// <returns></returns>
        [HttpGet("user/list/{accountId}/{p}")]
        public IActionResult GetList([FromRoute]int accountId, [FromRoute]int p)
        {
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var resultData = repository.Query()
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
                .Where(q => q.StateCode == 1 && q.AccountId== accountId)
                .OrderByDescending(q => q.ContentsId)
               .Skip(10 * (p - 1))
               .Take(10)
               .ToList();

            return APIReturnMethod.ReturnSuccess(resultData);
        }
        /// <summary>
        /// 根据ID获取内容
        /// </summary>
        /// <param name="contentsId">内容ID</param>
        /// <returns></returns>
        [HttpGet("{contentsId}")]
        public IActionResult Get(int contentsId)
        {
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var resultData = repository.Query()
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
                .Where(q => q.StateCode == 1 && q.ContentsId == contentsId)
                .OrderByDescending(q => q.ContentsId)
                .FirstOrDefault();
            return APIReturnMethod.ReturnSuccess(resultData);
        }
        /// <summary>
        /// 根据自定义类型获取
        /// </summary>
        /// <param name="type">new 表示获取最新的,hot 表示获去热门</param>
        /// <param name="count">需要获取的记录条数</param>
        /// <returns></returns>
        [HttpGet("customize/{type}/{count}")]
        public IActionResult Get(string type,int count)
        {
            var repository = _unitOfWork.GetRepository<Entity.m_CmsContents>();
            var channelRepository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var query = repository.Query()
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
                .Where(q => q.StateCode == 1);

            List<Models.ContentsListDataModel> resultData=new List<Models.ContentsListDataModel>();
            switch (type)
            {
                case "new":
                    resultData = query.OrderByDescending(q=>q.ContentsId).Take(count).ToList();
                    break;
                case "hot":
                    resultData = query.OrderByDescending(q => q.ReadCount).Where(q => q.PostTime >= DateTime.Now.AddDays(-7)).Take(10).ToList();
                    break;
            }
            return APIReturnMethod.ReturnSuccess(resultData);
        }
        /// <summary>
        /// 内容编辑
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(Models.ContentsEditRequestModel requestModel)
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
            Entity.m_CmsContents entity = repository.Query().Where(q=>q.ContentsId==requestModel.ContentsId).FirstOrDefault();
            entity.Contents = requestModel.Contents;//Framework.Core.HtmlFilter.SanitizeHtml(model.Contents);
            entity.LastTime = DateTime.Now;
            entity.Title = requestModel.Title;
            entity.ContentsId = requestModel.ContentsId;
            entity.ChannelId = requestModel.ChannelId;
            
            repository.Update(entity);
            int resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        /// <summary>
        /// 内容发布
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(Models.ContentReleaseRequestModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.Title)|| requestModel.Title=="")
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