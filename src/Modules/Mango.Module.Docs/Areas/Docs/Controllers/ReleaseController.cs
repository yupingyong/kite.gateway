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
    public class ReleaseController : Controller
    {
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

            string requestData = JsonConvert.SerializeObject(requestModel);
            var apiResult = HttpCore.HttpPost("/api/Docs/Theme", requestData);
            return Json(apiResult);
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

            string requestData = JsonConvert.SerializeObject(requestModel);
            var apiResult = HttpCore.HttpPost($"/api/Docs/Contents", requestData);
            return Json(apiResult);
        }

    }
}