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
    public class ThemeController : ControllerBase
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ThemeController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 获取主题文档列表
        /// </summary>
        /// <param name="accountId">用户账户ID</param>
        /// <param name="themeId">文档主题ID</param>
        /// <param name="p">页码</param>
        /// <returns></returns>
        [HttpGet("user/{accountId}/{themeId}/{p}")]
        public IActionResult Get([FromRoute]int accountId, [FromRoute]int themeId, [FromRoute] int p)
        {
            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            var docListData = docRepository.Query()
                    .Where(q => q.ThemeId == themeId && q.IsShow == true && q.AccountId == accountId)
                    .OrderByDescending(q => q.DocsId)
                    .Select(q => new Models.DocumentDataModel()
                    {
                        DocsId = q.DocsId.Value,
                        ShortTitle = q.ShortTitle,
                        Title = q.Title,
                        ThemeId = q.ThemeId.Value,
                        IsShow = q.IsShow.Value,
                        AppendTime=q.AppendTime,
                        PlusCount=q.PlusCount,
                        ReadCount=q.ReadCount,
                        AccountId=q.AccountId
                    })
                     .OrderByDescending(q => q.DocsId)
                     .Skip(10 * (p - 1))
                     .Take(10)
                     .ToList();
            return APIReturnMethod.ReturnSuccess(docListData);
        }
        /// <summary>
        /// 获取用户发布的文档主题列表
        /// </summary>
        /// <param name="accountId">用户账户ID</param>
        /// <param name="p">页码</param>
        /// <returns></returns>
        [HttpGet("user/{accountId}/{p}")]
        public IActionResult Get([FromRoute]int accountId, [FromRoute]int p)
        {
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var resultData = repository.Query()
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
            return APIReturnMethod.ReturnSuccess(resultData);
        }
        /// <summary>
        /// 文档主题编辑
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]Models.ThemeEditRequestModel requestModel)
        {
            if (requestModel.Title.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档主题标题");
            }
            if (requestModel.Contents.Trim().Length <= 0)
            {
                return APIReturnMethod.ReturnFailed("请输入文档主题内容");
            }
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();

            Entity.m_DocsTheme model = repository.Query().Where(q=>q.ThemeId==requestModel.ThemeId).FirstOrDefault();
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
        /// <summary>
        /// 创建新文档主题
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]Models.ThemeCreateRequestModel requestModel)
        {
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
            var resultCount= _unitOfWork.SaveChanges();
            return resultCount > 0 ? APIReturnMethod.ReturnSuccess() : APIReturnMethod.ReturnFailed();
        }
        /// <summary>
        /// 获取主题文档列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isGood"></param>
        /// <returns></returns>
        [HttpGet("document/{id}")]
        public IActionResult Get([FromRoute]int id,bool isGood)
        {
            var docRepository = _unitOfWork.GetRepository<Entity.m_Docs>();
            var docListData = docRepository.Query()
                    .Where(q => q.ThemeId == id && q.IsShow == true)
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
            return APIReturnMethod.ReturnSuccess(docListData);
        }
        /// <summary>
        /// 获取文档主题列表
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet("{p}")]
        public IActionResult Get([FromRoute]int p)
        {
            var repository = _unitOfWork.GetRepository<Entity.m_DocsTheme>();
            var accountRepository = _unitOfWork.GetRepository<m_Account>();
            var resultData = repository.Query()
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
            return APIReturnMethod.ReturnSuccess(resultData);
        }
    }
}