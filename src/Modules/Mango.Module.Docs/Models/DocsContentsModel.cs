using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Models
{
    public class DocsContentsModel
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
        /// 内容
        /// </summary>

        public string Contents { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>

        public DateTime AppendTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>

        public DateTime LastTime { get; set; }

        /// <summary>
        /// 所属文档主题
        /// </summary>

        public int ThemeId { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>

        public int AccountId { get; set; }

        /// <summary>
        /// 浏览数
        /// </summary>

        public int ReadCount { get; set; }

        /// <summary>
        /// +1数
        /// </summary>

        public int PlusCount { get; set; }

        /// <summary>
        /// 标签
        /// </summary>

        public string Tags { get; set; }

        /// <summary>
        /// 支持的版本信息
        /// </summary>

        public string VersionText { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>

        public bool IsShow { get; set; }

        /// <summary>
        /// 是否已经审核
        /// </summary>

        public bool IsAudit { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadUrl { get; set; }
    }
}
