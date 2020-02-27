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
using Microsoft.AspNetCore.SignalR;
namespace Mango.Module.CMS.Areas.CMS.Controllers
{
    public class PlusController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public PlusController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public IActionResult Add(Models.PlusRequestModel requestModel)
        {
            int accountId= HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            var plusRepository = _unitOfWork.GetRepository<m_AccountPlusRecords>();
            var plusCount = plusRepository.Query().Where(q => q.AccountId == accountId && q.ObjectId == requestModel.ContentsId && q.RecordsType == 1).Select(q => q.RecordsId).Count();

            int resultCount = 0;
            try
            {
                _unitOfWork.BeginTransaction();
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
                    message.Contents =Mango.Module.Core.Common.MessageHtml.GetMessageContent(HttpContext.Session.GetString("NickName"), requestModel.ContentsId, requestModel.Title, 12, 0);
                    message.IsRead = false;
                    message.MessageType = 12;
                    message.ObjectId = requestModel.ContentsId;
                    message.PostTime = DateTime.Now;
                    message.AccountId = requestModel.ToAccountId;
                    var messageRepository = _unitOfWork.GetRepository<m_Message>();
                    messageRepository.Insert(message);
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
            }
            catch 
            { }
            return APIReturnMethod.ReturnSuccess(resultCount);
        }
    }
}