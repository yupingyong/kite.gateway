using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Core.Entity
{
    public partial class m_Sms:EntityBase
    {
		
        /// <summary>
        /// 短信ID
        /// </summary>
        [Key]
        public int? SmsID { get; set; }
		
        /// <summary>
        /// 短信手机号
        /// </summary>
        
        public string Phone { get; set; }
		
        /// <summary>
        /// 短信内容
        /// </summary>
        
        public string Contents { get; set; }
		
        /// <summary>
        /// 发送时间
        /// </summary>
        
        public DateTime? SendTime { get; set; }
		
        /// <summary>
        /// 发送IP地址
        /// </summary>
        
        public string SendIP { get; set; }
		
        /// <summary>
        /// 是否发送成功
        /// </summary>
        
        public bool? IsOk { get; set; }
	
    }
}