using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.CMS.Entity
{
    public partial class m_CmsChannel:EntityBase
    {
		
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? ChannelId { get; set; }
		
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
        
        public int? StateCode { get; set; }
		
        /// <summary>
        /// 频道创建时间
        /// </summary>
        
        public DateTime? AppendTime { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public int? SortCount { get; set; }
		
    }
}