using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mango.Module.Core.Entity;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Mango.Framework.Services.Cache;
using Newtonsoft.Json;
namespace Mango.Module.Core.Common
{
    public class WebSite
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        private ICacheService _cacheService;
        public WebSite(IUnitOfWork<MangoDbContext> unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        /// <summary>
        /// 获取网站设置基础数据
        /// </summary>
        /// <returns></returns>
        public Models.WebSiteViewModel GetWebSiteData()
        {
            Models.WebSiteViewModel viewModel = new Models.WebSiteViewModel();
            var apiResult = HttpCore.HttpGet(ApiServerConfig.WebSite_GetWebSiteNavigation);
            if (apiResult.Code == 0)
            {
                viewModel.WebSiteNavigationData = JsonConvert.DeserializeObject<List<Models.WebSiteNavigationModel>>(apiResult.Data.ToString());
            }
            apiResult= HttpCore.HttpGet(ApiServerConfig.WebSite_GetWebSiteConfig);
            if (apiResult.Code == 0)
            {
                viewModel.WebSiteConfigData = JsonConvert.DeserializeObject<Models.WebSiteConfigModel>(apiResult.Data.ToString());
            }
            return viewModel;
        }
    }
}
