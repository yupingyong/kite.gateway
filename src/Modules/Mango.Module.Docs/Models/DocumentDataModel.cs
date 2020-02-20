using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Models
{
    public class DocumentDataModel
    {
        /// <summary>
        /// 文档项ID
        /// </summary>
        public int DocsId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>

        public string Title { get; set; }

        /// <summary>
        /// 短标题
        /// </summary>

        public string ShortTitle { get; set; }
        /// <summary>
        /// 所属文档主题
        /// </summary>

        public int ThemeId { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>

        public DateTime? AppendTime { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>

        public int? AccountId { get; set; }
        /// <summary>
        /// 浏览数
        /// </summary>

        public int? ReadCount { get; set; }

        /// <summary>
        /// +1数
        /// </summary>

        public int? PlusCount { get; set; }
    }
}
