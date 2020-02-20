using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.CMS.Entity
{
    public partial class m_CmsContents:EntityBase
    {
		
        /// <summary>
        /// 内容Id
        /// </summary>
        [Key]
        public int? ContentsId { get; set; }
		
        /// <summary>
        /// 标题
        /// </summary>
        
        public string Title { get; set; }
		
        /// <summary>
        /// 内容
        /// </summary>
        
        public string Contents { get; set; }
		
        /// <summary>
        /// 发布时间
        /// </summary>
        
        public DateTime? PostTime { get; set; }
		
        /// <summary>
        /// 最后更新时间
        /// </summary>
        
        public DateTime? LastTime { get; set; }
		
        /// <summary>
        /// 账号Id
        /// </summary>
        
        public int? AccountId { get; set; }
		
        /// <summary>
        /// +1数
        /// </summary>
        
        public int? PlusCount { get; set; }
		
        /// <summary>
        /// 阅读次数
        /// </summary>
        
        public int? ReadCount { get; set; }
		
        /// <summary>
        /// 是否显示
        /// </summary>
        
        public int? StateCode { get; set; }
		
        /// <summary>
        /// 内容标签
        /// </summary>
        
        public string Tags { get; set; }
		
        /// <summary>
        /// 图片地址
        /// </summary>
        
        public string ImgUrl { get; set; }
		
        /// <summary>
        /// 回复数
        /// </summary>
        
        public int? AnswerCount { get; set; }
		
        /// <summary>
        /// 所属频道
        /// </summary>
        
        public int? ChannelId { get; set; }
		
    }
}