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

namespace Mango.Module.Docs.Controllers
{
    [Area("Docs")]
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
        /// 文档内容编辑
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]Models.DocsContentsEditRequestModel requestModel)
        {
            if (requestModel.Title.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档标题");
            }
            if (requestModel.Contents.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档内容");
            }
            var repository = _unitOfWork.GetRepository<Entity.m_Docs>();

            Entity.m_Docs model = repository.Query().Where(q => q.DocsId == requestModel.DocsId).FirstOrDefault();
            if (model == null)
            {
                return APIReturnMethod.ReturnFailed("您要编辑的文档内容信息不存在!");
            }
            if (model.AccountId != requestModel.AccountId)
            {
                return APIReturnMethod.ReturnFailed("您无权对当前的数据进行编辑操作!");
            }
            model.Contents = HtmlFilter.SanitizeHtml(requestModel.Contents);
            model.LastTime = DateTime.Now;
            model.Title = HtmlFilter.StripHtml(requestModel.Title);
            model.ShortTitle = HtmlFilter.StripHtml(requestModel.ShortTitle);
            
            repository.Update(model);
            var resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        /// <summary>
        /// 创建新文档内容
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Models.DocsContentsCreateRequestModel requestModel)
        {
            if (requestModel.Title.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档标题");
            }
            if (requestModel.Contents.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档内容");
            }
            Entity.m_Docs model = new Entity.m_Docs();
            model.AppendTime = DateTime.Now;
            model.Contents = HtmlFilter.SanitizeHtml(requestModel.Contents);
            model.IsShow = true;
            model.LastTime = DateTime.Now;
            model.PlusCount = 0;
            model.ReadCount = 0;
            model.Tags = "";
            model.Title = HtmlFilter.StripHtml(requestModel.Title);
            model.AccountId = requestModel.AccountId;
            model.VersionText = "";
            model.ThemeId = requestModel.ThemeId;
            model.ShortTitle = HtmlFilter.StripHtml(requestModel.ShortTitle);
            model.IsAudit = true;
            var repository = _unitOfWork.GetRepository<Entity.m_Docs>();
            repository.Insert(model);
            var resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        /// <summary>
        /// 根据文档主题ID获取主题详情数据
        /// </summary>
        /// <param name="themeId">文档主题ID</param>
        /// <param name="docsId">文档ID</param>
        /// <returns></returns>
        [HttpGet("{themeId}/{docsId}")]
        public IActionResult Get([FromRoute]int themeId,[FromRoute]int docsId)
        {
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            var docsContentsData = docRepository.Query()
                           .Join(accountRepository.Query(), doc => doc.AccountId, acc => acc.AccountId, (doc, acc) => new Models.DocsContentsModel()
                           {
                               DocsId = doc.DocsId.Value,
                               HeadUrl = acc.HeadUrl,
                               IsShow = doc.IsShow.Value,
                               LastTime = doc.LastTime.Value,
                               PlusCount = doc.PlusCount.Value,
                               NickName = acc.NickName,
                               AppendTime = doc.AppendTime.Value,
                               ReadCount = doc.ReadCount.Value,
                               Title = doc.Title,
                               Tags = doc.Tags,
                               AccountId = doc.AccountId.Value,
                               ShortTitle = doc.ShortTitle,
                               ThemeId = doc.ThemeId.Value,
                               Contents = doc.Contents,
                               IsAudit = doc.IsAudit.Value
                           })
                           .Where(q => q.DocsId == docsId&&q.ThemeId==themeId)
                           .FirstOrDefault();
            if (docsContentsData != null)
            {
                //更新浏览次数
                _unitOfWork.DbContext.MangoUpdate<Entity.m_Docs>(q => q.ReadCount == q.ReadCount + 1, q => q.DocsId == docsId);
            }
            return APIReturnMethod.ReturnSuccess(docsContentsData);
        }
        /// <summary>
        /// 根据文档主题ID获取主题详情数据
        /// </summary>
        /// <param name="themeId">文档主题ID</param>
        /// <returns></returns>
        [HttpGet("{themeId}")]
        public IActionResult Get([FromRoute]int themeId)
        {
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var themeRepository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            var themeData = themeRepository.Query()
                .Join(accountRepository.Query(), doc => doc.AccountId, acc => acc.AccountId, (doc, acc) => new Models.ThemeDataModel()
                {
                    ThemeId = doc.ThemeId.Value,
                    HeadUrl = acc.HeadUrl,
                    IsShow = doc.IsShow.Value,
                    LastTime = doc.LastTime.Value,
                    PlusCount = doc.PlusCount.Value,
                    NickName = acc.NickName,
                    AppendTime = doc.AppendTime.Value,
                    ReadCount = doc.ReadCount.Value,
                    Title = doc.Title,
                    Tags = doc.Tags,
                    AccountId = doc.AccountId.Value,
                    Contents = doc.Contents
                })
               .Where(q => q.ThemeId == themeId)
               .OrderByDescending(q => q.ThemeId)
               .FirstOrDefault();
            if (themeData != null)
            {
                //更新浏览次数
                _unitOfWork.DbContext.MangoUpdate<Entity.m_DocsTheme>(q => q.ReadCount == q.ReadCount + 1, q => q.ThemeId == themeId);
            }
            return APIReturnMethod.ReturnSuccess(themeData);
        }

        /// <summary>
        /// 根据文档主题ID获取主题详情数据
        /// </summary>
        /// <param name="accountId">用户账户ID</param>
        /// <param name="themeId">文档主题ID</param>
        /// <param name="docsId">文档ID</param>
        /// <returns></returns>
        [HttpGet("user/{accountId}/{themeId}/{docsId}")]
        public IActionResult Get([FromRoute]int accountId,[FromRoute]int themeId, [FromRoute]int docsId)
        {
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            var docsContentsData = docRepository.Query()
                           .Join(accountRepository.Query(), doc => doc.AccountId, acc => acc.AccountId, (doc, acc) => new Models.DocsContentsModel()
                           {
                               DocsId = doc.DocsId.Value,
                               HeadUrl = acc.HeadUrl,
                               IsShow = doc.IsShow.Value,
                               LastTime = doc.LastTime.Value,
                               PlusCount = doc.PlusCount.Value,
                               NickName = acc.NickName,
                               AppendTime = doc.AppendTime.Value,
                               ReadCount = doc.ReadCount.Value,
                               Title = doc.Title,
                               Tags = doc.Tags,
                               AccountId = doc.AccountId.Value,
                               ShortTitle = doc.ShortTitle,
                               ThemeId = doc.ThemeId.Value,
                               Contents = doc.Contents,
                               IsAudit = doc.IsAudit.Value
                           })
                           .Where(q => q.DocsId == docsId && q.ThemeId == themeId&&q.AccountId==accountId)
                           .FirstOrDefault();
            if (docsContentsData != null)
            {
                //更新浏览次数
                _unitOfWork.DbContext.MangoUpdate<Entity.m_Docs>(q => q.ReadCount == q.ReadCount + 1, q => q.DocsId == docsId);
            }
            return APIReturnMethod.ReturnSuccess(docsContentsData);
        }
        /// <summary>
        /// 根据文档主题ID获取主题详情数据
        /// </summary>
        /// <param name="accountId">用户账户ID</param>
        /// <param name="themeId">文档主题ID</param>
        /// <returns></returns>
        [HttpGet("user/{accountId}/{themeId}")]
        public IActionResult GetUserTheme([FromRoute]int accountId,[FromRoute]int themeId)
        {
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var themeRepository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            var themeData = themeRepository.Query()
                .Join(accountRepository.Query(), doc => doc.AccountId, acc => acc.AccountId, (doc, acc) => new Models.ThemeDataModel()
                {
                    ThemeId = doc.ThemeId.Value,
                    HeadUrl = acc.HeadUrl,
                    IsShow = doc.IsShow.Value,
                    LastTime = doc.LastTime.Value,
                    PlusCount = doc.PlusCount.Value,
                    NickName = acc.NickName,
                    AppendTime = doc.AppendTime.Value,
                    ReadCount = doc.ReadCount.Value,
                    Title = doc.Title,
                    Tags = doc.Tags,
                    AccountId = doc.AccountId.Value,
                    Contents = doc.Contents
                })
               .Where(q => q.ThemeId == themeId&&q.AccountId== accountId)
               .OrderByDescending(q => q.ThemeId)
               .FirstOrDefault();
            return APIReturnMethod.ReturnSuccess(themeData);
        }
    }
}