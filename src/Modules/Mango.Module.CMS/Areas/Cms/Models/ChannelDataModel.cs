using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Cms.Areas.Cms.Models
{
    public class ChannelDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 频道名称
        /// </summary>

        public string ChannelName { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>

        public string RemarkText { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>

        public int StateCode { get; set; }

        /// <summary>
        /// 频道创建时间
        /// </summary>

        public DateTime AppendTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>

        public int SortCount { get; set; }
    }
}
