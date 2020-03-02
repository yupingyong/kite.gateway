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
using Mango.Framework.Services.RabbitMQ;

namespace Mango.Module.Docs.Areas.Docs.Controllers
{
    [Area("Docs")]
    public class PlusController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        private IRabbitMQService _rabbitMQService;
        public PlusController(IUnitOfWork<MangoDbContext> unitOfWork, IRabbitMQService rabbitMQService)
        {
            _unitOfWork = unitOfWork;
            _rabbitMQService = rabbitMQService;
        }
        /// <summary>
        /// 文档主题点赞
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Theme(Models.ThemePlusRequestModel requestModel)
        {
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var messageRepository = _unitOfWork.GetRepository<m_Message>();
            var plusRepository = _unitOfWork.GetRepository<m_AccountPlusRecords>();
            var plusCount = plusRepository.Query().Where(q => q.AccountId == accountId && q.ObjectId == requestModel.ThemeId && q.RecordsType == 10).Select(q => q.RecordsId).Count();

            int resultCount = 0;
            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {

                    if (plusCount > 0)
                    {
                        //存在则撤回点赞记录
                        _unitOfWork.DbContext.MangoRemove<m_AccountPlusRecords>(q => q.ObjectId == requestModel.ThemeId && q.AccountId == accountId && q.RecordsType == 10);
                        //
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_DocsTheme>(q => q.PlusCount == q.PlusCount - 1, q => q.ThemeId == requestModel.ThemeId);
                        resultCount = -1;
                    }
                    else
                    {

                        //添加新点赞记录
                        m_AccountPlusRecords model = new m_AccountPlusRecords();
                        model.ObjectId = requestModel.ThemeId;
                        model.AppendTime = DateTime.Now;
                        model.RecordsType = 10;
                        model.AccountId = accountId;
                        plusRepository.Insert(model);
                        //
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_DocsTheme>(q => q.PlusCount == q.PlusCount + 1, q => q.ThemeId == requestModel.ThemeId);
                        //
                        //消息通知
                        m_Message message = new m_Message();
                        message.AppendAccountId = accountId;
                        message.Contents = Core.Common.MessageHtml.GetMessageContent(HttpContext.Session.GetString("NickName"), requestModel.ThemeId, requestModel.Title, 10, 0);
                        message.IsRead = false;
                        message.MessageType = 10;
                        message.ObjectId = requestModel.ThemeId;
                        message.PostTime = DateTime.Now;
                        message.AccountId = requestModel.ToAccountId;

                        messageRepository.Insert(message);
                        resultCount = 1;
                    }
                    _unitOfWork.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    resultCount = 0;
                }
            }
            if (resultCount > 0)
            {
                var messageCount = messageRepository.Query().Where(q => q.AccountId == requestModel.ToAccountId && q.IsRead == false).Select(q => q.MessageId).Count();
                string sendMsg = $"{accountId}#{messageCount}";
                //发送消息
                _rabbitMQService.BasicPublish("message", System.Text.Encoding.UTF8.GetBytes(sendMsg));
            }
            return APIReturnMethod.ReturnSuccess(resultCount);
        }
        /// <summary>
        /// 文档内容点赞
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Docs(Models.DocsPlusRequestModel requestModel)
        {
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var messageRepository = _unitOfWork.GetRepository<m_Message>();
            var plusRepository = _unitOfWork.GetRepository<m_AccountPlusRecords>();

            var plusCount = plusRepository.Query().Where(q => q.AccountId == accountId && q.ObjectId == requestModel.DocsId && q.RecordsType == 11).Select(q => q.RecordsId).Count();

            int resultCount = 0;
            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (plusCount > 0)
                    {
                        //存在则撤回点赞记录
                        _unitOfWork.DbContext.MangoRemove<m_AccountPlusRecords>(q => q.ObjectId == requestModel.DocsId && q.AccountId == accountId && q.RecordsType == 11);
                        //
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_Docs>(q => q.PlusCount == q.PlusCount - 1, q => q.DocsId == requestModel.DocsId);
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_DocsTheme>(q => q.PlusCount == q.PlusCount - 1, q => q.ThemeId == requestModel.ThemeId);
                        resultCount = -1;
                    }
                    else
                    {

                        //添加新点赞记录
                        m_AccountPlusRecords model = new m_AccountPlusRecords();
                        model.ObjectId = requestModel.DocsId;
                        model.AppendTime = DateTime.Now;
                        model.RecordsType = 11;
                        model.AccountId = accountId;
                        plusRepository.Insert(model);
                        //
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_Docs>(q => q.PlusCount == q.PlusCount + 1, q => q.DocsId == requestModel.DocsId);
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_DocsTheme>(q => q.PlusCount == q.PlusCount + 1, q => q.ThemeId == requestModel.ThemeId);
                        //
                        //消息通知
                        m_Message message = new m_Message();
                        message.AppendAccountId = accountId;
                        message.Contents = Core.Common.MessageHtml.GetMessageContent(HttpContext.Session.GetString("NickName"), requestModel.ThemeId, requestModel.Title, 11, 0);
                        message.IsRead = false;
                        message.MessageType = 11;
                        message.ObjectId = requestModel.ThemeId;
                        message.PostTime = DateTime.Now;
                        message.AccountId = requestModel.ToAccountId;

                        messageRepository.Insert(message);
                        resultCount = 1;
                    }
                    _unitOfWork.SaveChanges();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    resultCount = 0;
                }
            }
            if (resultCount > 0)
            {
                var messageCount = messageRepository.Query().Where(q => q.AccountId == requestModel.ToAccountId && q.IsRead == false).Select(q => q.MessageId).Count();
                string sendMsg = $"{accountId}#{messageCount}";
                //发送消息
                _rabbitMQService.BasicPublish("message", System.Text.Encoding.UTF8.GetBytes(sendMsg));
            }
            return APIReturnMethod.ReturnSuccess(resultCount);
        }
    }
}