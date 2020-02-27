using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Core.Entity
{
    public partial class m_AccountPlusRecords: EntityBase
    {
		
        /// <summary>
        /// 记录Id
        /// </summary>
        [Key]
        public int? RecordsId { get; set; }
		
        /// <summary>
        /// 点赞对象Id
        /// </summary>
        
        public int? ObjectId { get; set; }
		
        /// <summary>
        /// 账号ID
        /// </summary>
        
        public int? AccountId { get; set; }
		
        /// <summary>
        /// 添加时间
        /// </summary>
        
        public DateTime? AppendTime { get; set; }
		
        /// <summary>
        /// 记录类型 1 帖子点赞 2 帖子回答点赞 3 帖子评论点赞 4 文档主题点赞 5 文档点赞
        /// </summary>
        
        public int? RecordsType { get; set; }
		
    }
}