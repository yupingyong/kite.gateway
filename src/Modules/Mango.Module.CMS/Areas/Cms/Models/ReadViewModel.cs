using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.CMS.Areas.CMS.Models
{
    public class ReadViewModel
    {
        public ContentsDataModel ContentsData { get; set; }
        /// <summary>
        /// 帖子频道
        /// </summary>
        public List<ChannelDataModel> ChannelListData { get; set; }
        /// <summary>
        /// 一周热门帖子
        /// </summary>
        public List<ContentsListDataModel> HotListData { get; set; }
    }
}
