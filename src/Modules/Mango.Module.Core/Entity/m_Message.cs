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
        /// 帖子消息: 10:帖子回复消息,11:帖子评论消息,12:帖子点赞消息,13:回复点赞消息,14:评论点赞消息
        /// </summary>

        public int? MessageType { get; set; }
		
        /// <summary>
        /// 内容
        /// </summary>
        
        public string Contents { get; set; }
		
        /// <summary>
        /// 提交时间
        /// </summary>
        
        public DateTime? PostDate { get; set; }
		
        /// <summary>
        /// 消息接收用户
        /// </summary>
        
        public int? UserId { get; set; }
		
        /// <summary>
        /// 产生消息用户
        /// </summary>
        
        public int? AppendUserId { get; set; }
		
        /// <summary>
        /// 存储对象Id(PostId ShareId)
        /// </summary>
        
        public int? ObjId { get; set; }
		
        /// <summary>
        /// 是否已经阅读
        /// </summary>
        
        public bool? IsRead { get; set; }
		
    }
}