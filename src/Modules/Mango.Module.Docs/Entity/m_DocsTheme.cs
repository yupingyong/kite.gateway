using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Docs.Entity
{
    public partial class m_DocsTheme:EntityBase
    {
		
        /// <summary>
        /// 文档ID
        /// </summary>
        [Key]
        public int? ThemeId { get; set; }
		
        /// <summary>
        /// 标题
        /// </summary>
        
        public string Title { get; set; }
		
        /// <summary>
        /// 内容描述
        /// </summary>
        
        public string Contents { get; set; }
		
        /// <summary>
        /// 发布用户
        /// </summary>
        
        public int? AccountId { get; set; }
		
        /// <summary>
        /// 添加时间
        /// </summary>
        
        public DateTime? AppendTime { get; set; }
		
        /// <summary>
        /// 最后更新时间
        /// </summary>
        
        public DateTime? LastTime { get; set; }
		
        /// <summary>
        /// 浏览数
        /// </summary>
        
        public int? ReadCount { get; set; }
		
        /// <summary>
        /// +1数
        /// </summary>
        
        public int? PlusCount { get; set; }
		
        /// <summary>
        /// 标签
        /// </summary>
        
        public string Tags { get; set; }
		
        /// <summary>
        /// 版本号信息
        /// </summary>
        
        public string VersionText { get; set; }
		
        /// <summary>
        /// 是否显示
        /// </summary>
        
        public bool? IsShow { get; set; }
		
    }
}