using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.CMS.Areas.CMS.Models
{
    public class ChannelViewModel
    {
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页数据返回集合
        /// </summary>
        public List<ContentsListDataModel> ContentsListData { get; set; }
        /// <summary>
        /// 帖子频道
        /// </summary>
        public List<ChannelDataModel> ChannelListData { get; set; }
    }
}
