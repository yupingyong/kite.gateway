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
    public class EditController : Controller
    {
        [HttpGet("{area}/{controller}/{id}")]
        public IActionResult Index(int id)
        {
            Models.EditViewModel viewModel = new Models.EditViewModel();
            //获取频道数据
            var apiResult = HttpCore.HttpGet($"/api/CMS/Channel");
            if (apiResult.Code == 0)
            {
                viewModel.ChannelListData = JsonConvert.DeserializeObject<List<Models.ChannelDataModel>>(apiResult.Data.ToString());
            }
            //获取文章内容数据
            int accountId= HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);
            apiResult = HttpCore.HttpGet($"/api/CMS/Contents/user/{accountId}/{id}");
            if (apiResult.Code == 0)
            {
                viewModel.ContentsData = JsonConvert.DeserializeObject<Models.ContentsDataModel>(apiResult.Data.ToString());
            }
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Index(Models.ContentsEditRequestModel requestModel)
        {
            //requestModel.AccountId = HttpContext.Session.GetInt32("AccountId").GetValueOrDefault(0);

            string requestData = JsonConvert.SerializeObject(requestModel);
            var apiResult = HttpCore.HttpPut($"/api/CMS/Contents", requestData);
            return Json(apiResult);
        }
    }
}