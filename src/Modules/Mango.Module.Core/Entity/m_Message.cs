using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Core.Entity
{
    public partial class m_Message:EntityBase
    {
		
        /// <summary>
        /// 消息Id
        /// </summary>
        [Key]
        public int? MessageId { get; set; }

        /// <summary>
        /// 消息类型
        /// 系统消息: 0:系统通知
        /// 其它消息: 1.文章点赞消息,10:文档主题点赞消息,11:文档点赞消息
        /// </summary>

        public int? MessageType { get; set; }
		
        /// <summary>
        /// 内容
        /// </summary>
        
        public string Contents { get; set; }
		
        /// <summary>
        /// 提交时间
        /// </summary>
        
        public DateTime? PostTime { get; set; }
		
        /// <summary>
        /// 消息接收账号ID
        /// </summary>
        
        public int? AccountId { get; set; }
		
        /// <summary>
        /// 产生消息账号ID
        /// </summary>
        
        public int? AppendAccountId { get; set; }
		
        /// <summary>
        /// 存储对象Id
        /// </summary>
        
        public int? ObjectId { get; set; }
		
        /// <summary>
        /// 是否已经阅读
        /// </summary>
        
        public bool? IsRead { get; set; }
		
    }
}