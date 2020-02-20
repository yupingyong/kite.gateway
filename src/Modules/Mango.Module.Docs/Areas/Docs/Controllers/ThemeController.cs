using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Mango.Module.Docs.Areas.Docs.Controllers
{
    [Area("Docs")]
    public class ThemeController : Controller
    {
        [Route("{area}/{controller}")]
        [Route("{area}/{controller}/{p}")]
        public IActionResult Index([FromRoute]int p=1)
        {
            Models.ThemeViewModel viewModel = new Models.ThemeViewModel();
            //获取数据
            var apiResult = HttpCore.HttpGet($"/api/Docs/Theme/{p}");

            if (apiResult.Code == 0)
            {
                viewModel.ThemeListData = JsonConvert.DeserializeObject<List<Models.ThemeDataModel>>(apiResult.Data.ToString());
            }
            return View(viewModel);
        }
    }
}