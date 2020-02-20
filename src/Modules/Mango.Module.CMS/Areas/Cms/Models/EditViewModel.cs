using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Cms.Areas.Cms.Models
{
    public class EditViewModel
    {
        /// <summary>
        /// 文章内容数据
        /// </summary>
        public ContentsDataModel ContentsData { get; set; }
        /// <summary>
        /// 频道数据
        /// </summary>
        public List<ChannelDataModel> ChannelListData { get; set; }
    }
}
