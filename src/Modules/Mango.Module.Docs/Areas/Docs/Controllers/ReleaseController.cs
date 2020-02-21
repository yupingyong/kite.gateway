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
    public class ReleaseController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ReleaseController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 发布文档主题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Theme()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Theme(Models.ThemeCreateRequestModel requestModel)
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
            Entity.m_DocsTheme model = new Entity.m_DocsTheme();
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
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            repository.Insert(model);
            var resultCount = _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        /// <summary>
        /// 发布文档
        /// </summary>
        /// <returns></returns>
        [HttpGet("{area}/{controller}/{action}/{themeId}")]
        public IActionResult Document(int themeId)
        {
            ViewData["ThemeId"] = themeId;
            return View();
        }
        [HttpPost]
        public IActionResult Document(Models.DocsContentsCreateRequestModel requestModel)
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

    }
}