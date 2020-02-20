using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Mango.Module.Core.Models
{
    public class WebSiteViewModel
    {
        /// <summary>
        /// 网站基础配置数据
        /// </summary>
        public WebSiteConfigModel WebSiteConfigData { get; set; }
        /// <summary>
        /// 顶部导航数据
        /// </summary>
        public List<WebSiteNavigationModel> WebSiteNavigationData { get; set; }

    }
}
