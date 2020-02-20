using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Mango.Module.Core.Entity;
namespace Mango.Module.CMS.Controllers
{
    [Area("CMS")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ChannelController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 根据频道获取内容列表
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet("{channelId}/{p}")]
        public IActionResult Get(int channelId, int p)
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
                .Where(q => q.StateCode == 1 && (channelId > 0 ? q.ChannelId == channelId : q.ChannelId != 0))
                .OrderByDescending(q => q.ContentsId)
               .Skip(10 * (p - 1))
               .Take(10)
               .ToList();

            return APIReturnMethod.ReturnSuccess(resultData);
        }
        /// <summary>
        /// 获取频道数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var repository = _unitOfWork.GetRepository<Entity.m_CmsChannel>();
            var resultData = repository.Query().OrderBy(q => q.SortCount).ToList();
            return APIReturnMethod.ReturnSuccess(resultData);
        }
    }
}