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
    public class ThemeController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ThemeController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("{area}/{controller}")]
        [Route("{area}/{controller}/{p}")]
        public IActionResult Index([FromRoute]int p=1)
        {
            Models.ThemeViewModel viewModel = new Models.ThemeViewModel();
            //获取数据
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            viewModel.ThemeListData = repository.Query()
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
                .OrderByDescending(q => q.ThemeId)
                .Skip(10 * (p - 1))
                .Take(10)
                .ToList();
            return View(viewModel);
        }
    }
}