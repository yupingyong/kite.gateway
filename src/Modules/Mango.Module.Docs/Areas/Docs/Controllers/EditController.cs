using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Http;
namespace Mango.Module.Docs.Areas.Docs.Controllers
{
    [Area("Docs")]
    public class EditController : Controller
    {
        [HttpGet]
        public IActionResult Theme(int id)
        {
            Models.EditThemeViewModel viewModel = new Models.EditThemeViewModel();
            //获取文章内容数据
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var apiResult = HttpCore.HttpGet($"/api/Docs/Contents/user/{accountId}/{id}");
            if (apiResult.Code == 0)
            {
                viewModel.ThemeData = JsonConvert.DeserializeObject<Models.ThemeDataModel>(apiResult.Data.ToString());
            }
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Theme(Models.EditThemeRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            string requestData = JsonConvert.SerializeObject(requestModel);
            var apiResult = HttpCore.HttpPut($"/api/Docs/Theme", requestData);
            return Json(apiResult);
        }
        [HttpGet("{area}/{controller}/{action}/{themeId}/{docsId}")]
        public IActionResult Document(int themeId,int docsId)
        {
            Models.EditDocumentViewModel viewModel = new Models.EditDocumentViewModel();
            //获取文章内容数据
            int accountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            var apiResult = HttpCore.HttpGet($"/api/Docs/Contents/user/{accountId}/{themeId}/{docsId}");
            if (apiResult.Code == 0)
            {
                viewModel.DocsContentsData = JsonConvert.DeserializeObject<Models.DocsContentsModel>(apiResult.Data.ToString());
            }
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Document(Models.EditDocumentRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            string requestData = JsonConvert.SerializeObject(requestModel);
            var apiResult = HttpCore.HttpPut($"/api/Docs/Contents", requestData);
            return Json(apiResult);
        }
    }
}