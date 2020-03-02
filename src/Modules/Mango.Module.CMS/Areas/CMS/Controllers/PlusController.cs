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
namespace Mango.Module.CMS.Areas.CMS.Controllers
{
    [Area("CMS")]
    public class PlusController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        private IRabbitMQService _rabbitMQService;
        public PlusController(IUnitOfWork<MangoDbContext> unitOfWork, IRabbitMQService rabbitMQService)
        {
            _unitOfWork = unitOfWork;
            _rabbitMQService = rabbitMQService;
        }
        [HttpPost]
        public IActionResult Add(Models.PlusRequestModel requestModel)
        {
            int accountId= HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var messageRepository = _unitOfWork.GetRepository<m_Message>();
            var plusRepository = _unitOfWork.GetRepository<m_AccountPlusRecords>();
            var plusCount = plusRepository.Query().Where(q => q.AccountId == accountId && q.ObjectId == requestModel.ContentsId && q.RecordsType == 1).Select(q => q.RecordsId).Count();

            int resultCount = 0;
            using (var tran= _unitOfWork.BeginTransaction())
            {
                try
                {

                    if (plusCount > 0)
                    {
                        //存在则撤回点赞记录
                        _unitOfWork.DbContext.MangoRemove<m_AccountPlusRecords>(q => q.ObjectId == requestModel.ContentsId && q.AccountId == accountId && q.RecordsType == 1);
                        //
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_CmsContents>(q => q.PlusCount == q.PlusCount - 1, q => q.ContentsId == requestModel.ContentsId);
                        resultCount = -1;
                    }
                    else
                    {

                        //添加新点赞记录
                        m_AccountPlusRecords model = new m_AccountPlusRecords();
                        model.ObjectId = requestModel.ContentsId;
                        model.AppendTime = DateTime.Now;
                        model.RecordsType = 1;
                        model.AccountId = accountId;
                        plusRepository.Insert(model);
                        //
                        _unitOfWork.DbContext.MangoUpdate<Entity.m_CmsContents>(q => q.PlusCount == q.PlusCount + 1, q => q.ContentsId == requestModel.ContentsId);
                        //
                        //消息通知
                        m_Message message = new m_Message();
                        message.AppendAccountId = accountId;
                        message.Contents = Core.Common.MessageHtml.GetMessageContent(HttpContext.Session.GetString("NickName"), requestModel.ContentsId, requestModel.Title, 1, 0);
                        message.IsRead = false;
                        message.MessageType = 1;
                        message.ObjectId = requestModel.ContentsId;
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