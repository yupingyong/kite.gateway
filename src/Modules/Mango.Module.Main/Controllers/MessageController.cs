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
namespace Mango.Module.Main.Controllers
{
    public class MessageController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        private IRabbitMQService _rabbitMQService;
        public MessageController(IUnitOfWork<MangoDbContext> unitOfWork, IRabbitMQService rabbitMQService)
        {
            _unitOfWork = unitOfWork;
            _rabbitMQService = rabbitMQService;
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMessageList()
        {
            int accountId= HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var repository = _unitOfWork.GetRepository<m_Message>();
            var queryResult = repository.Query().Where(q => q.AccountId == accountId).ToList();

            return Json(queryResult);
        }
        /// <summary>
        /// 更新消息阅读状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateMessageReadState()
        {

            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var repository = _unitOfWork.GetRepository<m_Message>();
            int count = _unitOfWork.DbContext.MangoUpdate<m_Message>(q => q.IsRead == true, q => q.AccountId == accountId && q.IsRead == false);
            if (count>0)
            {
                //消息推送
                var messageCount = repository.Query().Where(q => q.AccountId == accountId && q.IsRead == false).Select(q => q.MessageId).Count();
                string sendMsg = $"{accountId}#{messageCount}";
                //发送消息
                _rabbitMQService.BasicPublish("message", System.Text.Encoding.UTF8.GetBytes(sendMsg));
            }
            return true;
        }
    }
}
