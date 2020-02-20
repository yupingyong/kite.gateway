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
    public class ChannelController : Controller
    {
        [Route("{area}/{controller}")]
        [Route("{area}/{controller}/{p}")]
        public IActionResult Index([FromRoute]int p = 1)
        {
            var viewModel = LoadMainData(0, p);

            return View(viewModel);
        }
        [Route("{area}/{controller}/{action}/{id}")]
        [Route("{area}/{controller}/{action}/{id}/{p}")]
        public IActionResult List([FromRoute]int id = 0,[FromRoute]int p=1)
        {
            var viewModel = LoadMainData(id, p);
            return View("~/Areas/Cms/Views/Channel/Index.cshtml",viewModel);
        }
        public Models.ChannelViewModel LoadMainData(int id,int p)
        {
            Models.ChannelViewModel viewModel = new Models.ChannelViewModel();
            //获取频道数据
            var apiResult = HttpCore.HttpGet("/api/CMS/Channel");

            if (apiResult.Code == 0)
            {
                viewModel.ChannelListData = JsonConvert.DeserializeObject<List<Models.ChannelDataModel>>(apiResult.Data.ToString());
            }

            //获取帖子数据

            apiResult = HttpCore.HttpGet($"/api/CMS/Channel/{id}/{p}");

            if (apiResult.Code == 0)
            {
                viewModel.ContentsListData = JsonConvert.DeserializeObject<List<Models.ContentsListDataModel>>(apiResult.Data.ToString());
            }
            return viewModel;
        }
    }
}