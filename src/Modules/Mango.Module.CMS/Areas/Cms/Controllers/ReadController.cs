using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mango.Module.Cms.Areas.Cms.Controllers
{
    [Area("Cms")]
    public class ReadController : Controller
    {
        [Route("cms/read/{id}")]
        [HttpGet]
        public IActionResult Index(int id)
        {
            Models.ReadViewModel viewModel = new Models.ReadViewModel();
            //获取帖子详情数据
            var requestData = new Dictionary<string, object>();
            var apiResult = HttpCore.HttpGet($"/api/CMS/Contents/{id}");
            if (apiResult.Code == 0)
            {
                viewModel.ContentsData = JsonConvert.DeserializeObject<Models.ContentsDataModel>(apiResult.Data.ToString());
            }
            //获取热门帖子数据
            apiResult = HttpCore.HttpGet($"/api/CMS/Contents/customize/hot/8");
            if (apiResult.Code == 0)
            {
                viewModel.HotListData = JsonConvert.DeserializeObject<List<Models.ContentsListDataModel>>(apiResult.Data.ToString());
            }
            //获取频道数据
            apiResult = HttpCore.HttpGet("/api/CMS/Channel");

            if (apiResult.Code == 0)
            {
                viewModel.ChannelListData = JsonConvert.DeserializeObject<List<Models.ChannelDataModel>>(apiResult.Data.ToString());
            }
            return View(viewModel);
        }
    }
}