using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Mango.Module.Docs.Areas.Docs.Models
{
    public class DocsReadViewModel
    {
        /// <summary>
        /// 文档ID
        /// </summary>
        public int DocsId { get; set; }
        /// <summary>
        /// 文档主题ID
        /// </summary>
        public int ThemeId { get; set; }
        /// <summary>
        /// 文档主题数据
        /// </summary>
        public Models.ThemeDataModel DocsThemeData { get; set; }
        /// <summary>
        /// 文档数据
        /// </summary>
        public Models.DocsContentsModel DocsData { get; set; }
        /// <summary>
        /// 文档列表数据
        /// </summary>
        public List<Models.DocumentDataModel> ItemsListData { get; set; }
    }
}
