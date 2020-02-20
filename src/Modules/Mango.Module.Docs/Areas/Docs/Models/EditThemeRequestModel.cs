using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Areas.Docs.Models
{
    public class EditThemeRequestModel
    {
        /// <summary>
        /// 文档主题ID
        /// </summary>
        public int ThemeId { get; set; }
        /// <summary>
        /// 文档标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文档内容
        /// </summary>
        public string Contents { get; set; }
        /// <summary>
        /// 发布用户
        /// </summary>
        public int AccountId { get; set; }
    }
}
