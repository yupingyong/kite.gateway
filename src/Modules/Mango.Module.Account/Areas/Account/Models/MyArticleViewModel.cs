using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Account.Areas.Account.Models
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
