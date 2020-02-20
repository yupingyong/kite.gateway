using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Module.Docs.Areas.Docs.Controllers
{
    public class ReadController : Controller
    {
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
            var apiResult = HttpCore.HttpGet($"/api/Docs/Theme/document/{themeId}");
            if (apiResult.Code == 0 && apiResult.Data != null)
            {
                viewModel.ItemsListData = JsonConvert.DeserializeObject<List<Models.DocumentDataModel>>(apiResult.Data.ToString());
            }
            else
            {
                viewModel.ItemsListData = new List<Models.DocumentDataModel>();
            }
            //获取帖子详情数据
            if (docsId == 0)
            {
                apiResult = HttpCore.HttpGet($"/api/Docs/Contents/{themeId}");
                if (apiResult.Code == 0)
                {
                    viewModel.DocsThemeData = JsonConvert.DeserializeObject<Models.ThemeDataModel>(apiResult.Data.ToString());
                }
            }
            else
            {
                apiResult = HttpCore.HttpGet($"/api/Docs/Contents/{themeId}/{docsId}");
                if (apiResult.Code == 0)
                {
                    viewModel.DocsData = JsonConvert.DeserializeObject<Models.DocsContentsModel>(apiResult.Data.ToString());
                }
            }
            return View(viewModel);
        }
    }
}