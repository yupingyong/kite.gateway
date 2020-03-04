using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.CMS.Areas.CMS.Models
{
    public class MyArticleViewModel
    {
        /// <summary>
        /// 分页返回总记录
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<ContentsListDataModel> ListData { get; set; }
    }
}
