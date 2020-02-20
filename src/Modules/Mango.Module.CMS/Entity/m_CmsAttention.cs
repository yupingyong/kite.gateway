using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.CMS.Entity
{
    public partial class m_CmsAttention:EntityBase
    {
		
        /// <summary>
        /// 关注Id
        /// </summary>
        [Key]
        public int? AttentionId { get; set; }
		
        /// <summary>
        /// 帖子Id
        /// </summary>
        
        public int? ContentsId { get; set; }
		
        /// <summary>
        /// 关注时间
        /// </summary>
        
        public DateTime? AttentionTime { get; set; }
		
        /// <summary>
        /// 用户Id
        /// </summary>
        
        public int? AccountId { get; set; }
		
    }
}