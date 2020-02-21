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
    public class ReadController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ReadController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 文档阅读浏览
        /// </summary>
        /// <returns></returns>
        [Route("{area}/{controller}/{themeId}")]
        [Route("{area}/{controller}/{themeId}/{docsId}")]
        public IActionResult Index([FromRoute]int themeId,[FromRoute]int docsId=0)
        {
            Models.DocsReadViewModel viewModel = new Models.DocsReadViewModel();
            viewModel.ThemeId = themeId;
            viewModel.DocsId = docsId;
            //
            //获取文档列表数据
            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            viewModel.ItemsListData = docRepository.Query()
                    .Where(q => q.ThemeId == themeId && q.IsShow == true)
                    .OrderByDescending(q => q.DocsId)
                    .Select(q => new Models.DocumentDataModel()
                    {
                        DocsId = q.DocsId.Value,
                        ShortTitle = q.ShortTitle,
                        Title = q.Title,
                        ThemeId = q.ThemeId.Value,
                        IsShow = q.IsShow.Value
                    })
                    .ToList();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            //获取帖子详情数据
            if (docsId == 0)
            {
                var themeRepository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
                viewModel.DocsThemeData = themeRepository.Query()
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
                if (viewModel.DocsThemeData != null)
                {
                    //更新浏览次数
                    _unitOfWork.DbContext.MangoUpdate<Entity.m_DocsTheme>(q => q.ReadCount == q.ReadCount + 1, q => q.ThemeId == themeId);
                }
            }
            else
            {
                viewModel.DocsData = docRepository.Query()
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
                           .Where(q => q.DocsId == docsId && q.ThemeId == themeId)
                           .FirstOrDefault();
                if (viewModel.DocsData != null)
                {
                    //更新浏览次数
                    _unitOfWork.DbContext.MangoUpdate<Entity.m_Docs>(q => q.ReadCount == q.ReadCount + 1, q => q.DocsId == docsId);
                }
            }
            return View(viewModel);
        }
    }
}