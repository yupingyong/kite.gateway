using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
namespace Mango.Module.Cms.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class ReleaseController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var apiResult = HttpCore.HttpGet("/api/CMS/Channel");
            if (apiResult.Code == 0)
            {
                var channelData = JsonConvert.DeserializeObject<List<Models.ChannelDataModel>>(apiResult.Data.ToString());
                return View(channelData);
            }
            return Json(apiResult);
        }
        [HttpPost]
        public IActionResult Index(Models.ReleaseRequestModel requestModel)
        {
            requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            string requestData = JsonConvert.SerializeObject(requestModel);
            var apiResult = HttpCore.HttpPost("​/api​/CMS​/Contents", requestData);
            return Json(apiResult);
        }
    }
}