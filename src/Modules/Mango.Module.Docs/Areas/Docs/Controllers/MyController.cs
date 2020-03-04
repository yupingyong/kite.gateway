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

namespace Mango.Module.Docs.Areas.Docs.Controllers
{
    [Area("Docs")]
    public class MyController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public MyController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("{area}/{controller}/{action}")]
        [Route("{area}/{controller}/{action}/{p}")]
        public IActionResult Theme([FromRoute]int p=1)
        {
            Models.MyThemeViewModel viewModel = new Models.MyThemeViewModel();
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            viewModel.ListData = repository.Query()
                .Join(accountRepository.Query(), t => t.AccountId, acc => acc.AccountId, (t, acc) => new Models.ThemeDataModel()
                {
                    ThemeId = t.ThemeId.Value,
                    HeadUrl = acc.HeadUrl,
                    IsShow = t.IsShow.Value,
                    LastTime = t.LastTime.Value,
                    PlusCount = t.PlusCount.Value,
                    NickName = acc.NickName,
                    AppendTime = t.AppendTime.Value,
                    ReadCount = t.ReadCount.Value,
                    Title = t.Title,
                    Tags = t.Tags,
                    AccountId = t.AccountId.Value
                })
                .Where(q => q.AccountId == accountId)
                .OrderByDescending(q => q.ThemeId)
                .Skip(10 * (p - 1))
                .Take(10)
                .ToList();
            return View(viewModel);
        }
        [HttpGet("{area}/{controller}/{action}/{themeId}")]
        [HttpGet("{area}/{controller}/{action}/{themeId}/{p}")]
        public IActionResult Document([FromRoute]int themeId, [FromRoute] int p=1)
        {
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            Models.MyThemeDocumentViewModel viewModel = new Models.MyThemeDocumentViewModel();

            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            viewModel.ListData = docRepository.Query()
                    .Where(q => q.ThemeId == themeId && q.IsShow == true && q.AccountId == accountId)
                    .OrderByDescending(q => q.DocsId)
                    .Select(q => new Models.DocumentDataModel()
                    {
                        DocsId = q.DocsId.Value,
                        ShortTitle = q.ShortTitle,
                        Title = q.Title,
                        ThemeId = q.ThemeId.Value,
                        IsShow = q.IsShow.Value,
                        AppendTime = q.AppendTime,
                        PlusCount = q.PlusCount,
                        ReadCount = q.ReadCount,
                        AccountId = q.AccountId
                    })
                     .OrderByDescending(q => q.DocsId)
                     .Skip(10 * (p - 1))
                     .Take(10)
                     .ToList();
            return View(viewModel);
        }
    }
}