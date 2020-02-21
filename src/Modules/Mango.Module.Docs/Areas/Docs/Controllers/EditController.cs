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
namespace Mango.Module.Docs.Areas.Docs.Controllers
{
    [Area("Docs")]
    public class EditController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public EditController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Theme(int id)
        {
            Models.EditThemeViewModel viewModel = new Models.EditThemeViewModel();
            //获取文章内容数据
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var themeRepository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            viewModel.ThemeData = themeRepository.Query()
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
               .Where(q => q.ThemeId == id && q.AccountId == accountId)
               .OrderByDescending(q => q.ThemeId)
               .FirstOrDefault();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Theme(Models.EditThemeRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            if (requestModel.Title.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档主题标题");
            }
            if (requestModel.Contents.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档主题内容");
            }
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();

            Entity.m_DocsTheme model = repository.Query().Where(q => q.ThemeId == requestModel.ThemeId).FirstOrDefault();
            if (model == null)
            {
                return APIReturnMethod.ReturnFailed("您要编辑的文档主题信息不存在!");
            }
            if (model.AccountId != requestModel.AccountId)
            {
                return APIReturnMethod.ReturnFailed("您无权对当前的数据进行编辑操作!");
            }
            model.Contents = HtmlFilter.SanitizeHtml(requestModel.Contents);
            model.LastTime = DateTime.Now;
            model.Title = HtmlFilter.StripHtml(requestModel.Title);
            model.VersionText = "";

            repository.Update(model);
            var resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        [HttpGet("{area}/{controller}/{action}/{themeId}/{docsId}")]
        public IActionResult Document(int themeId,int docsId)
        {
            Models.EditDocumentViewModel viewModel = new Models.EditDocumentViewModel();
            //获取文章内容数据
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            viewModel.DocsContentsData = docRepository.Query()
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
                           .Where(q => q.DocsId == docsId && q.ThemeId == themeId && q.AccountId == accountId)
                           .FirstOrDefault();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Document(Models.EditDocumentRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

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
    }
}