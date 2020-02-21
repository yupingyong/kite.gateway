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
        public WebSite()
        {
            _unitOfWork = Framework.Services.ServiceContext.GetService<IUnitOfWork<MangoDbContext>>();
            _cacheService = Framework.Services.ServiceContext.GetService<ICacheService>();
        }
        /// <summary>
        /// 获取网站设置基础数据
        /// </summary>
        /// <returns></returns>
        public Models.WebSiteViewModel GetWebSiteData()
        {
            Models.WebSiteViewModel viewModel = new Models.WebSiteViewModel();
            viewModel.WebSiteNavigationData = GetWebSiteNavigation();
            viewModel.WebSiteConfigData = GetWebSiteConfig();
            return viewModel;
        }
        /// <summary>
        /// 获取网站导航数据
        /// </summary>
        /// <returns></returns>
        private List<Models.WebSiteNavigationModel> GetWebSiteNavigation()
        {
            string cacheData = _cacheService.Get("WebSiteNavigationCache");
            if (string.IsNullOrEmpty(cacheData))
            {
                var repository = _unitOfWork.GetRepository<Entity.m_WebSiteNavigation>();
                var resultData = repository.Query()
                .OrderBy(nav => nav.SortCount)
                .Where(q => q.IsShow == true)
                .Select(nav => new Models.WebSiteNavigationModel()
                {
                    AppendTime = nav.AppendTime.Value,
                    IsShow = nav.IsShow.Value,
                    IsTarget = nav.IsTarget.Value,
                    LinkUrl = nav.LinkUrl,
                    NavigationId = nav.NavigationId.Value,
                    NavigationName = nav.NavigationName,
                    SortCount = nav.SortCount.Value
                }).ToList();
                cacheData = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultData)));
                //写入缓存
                _cacheService.Add("WebSiteNavigationCache", cacheData);
                return resultData;
            }
            else
            {
                cacheData = Encoding.UTF8.GetString(Convert.FromBase64String(cacheData.Replace("\"", "")));
                //从缓存中获取
                return JsonConvert.DeserializeObject<List<Models.WebSiteNavigationModel>>(cacheData);
            }
        }
        /// <summary>
        /// 获取网站配置数据信息
        /// </summary>
        /// <returns></returns>
        private Models.WebSiteConfigModel GetWebSiteConfig()
        {
            string cacheData = _cacheService.Get("WebSiteConfigCache");
            if (string.IsNullOrEmpty(cacheData))
            {
                var repository = _unitOfWork.GetRepository<Entity.m_WebSiteConfig>();
                var resultData = repository.Query()
                    .OrderBy(cfg => cfg.ConfigId)
                    .Select(cfg => new Models.WebSiteConfigModel()
                    {
                        ConfigId = cfg.ConfigId.Value,
                        CopyrightText = cfg.CopyrightText,
                        FilingNo = cfg.FilingNo,
                        IsLogin = cfg.IsLogin.Value,
                        IsRegister = cfg.IsRegister.Value,
                        WebSiteDescription = cfg.WebSiteDescription,
                        WebSiteKeyWord = cfg.WebSiteKeyWord,
                        WebSiteName = cfg.WebSiteName,
                        WebSiteTitle = cfg.WebSiteTitle,
                        WebSiteUrl = cfg.WebSiteUrl
                    })
                    .FirstOrDefault();
                cacheData = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultData)));
                //写入缓存
                _cacheService.Add("WebSiteConfigCache", cacheData);
                return resultData;
            }
            else
            {
                cacheData = Encoding.UTF8.GetString(Convert.FromBase64String(cacheData.Replace("\"", "")));
                return JsonConvert.DeserializeObject<Models.WebSiteConfigModel>(cacheData);
            }
        }
    }
}
