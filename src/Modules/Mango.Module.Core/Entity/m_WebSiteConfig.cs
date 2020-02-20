using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.WebSite.Entity
{
    public partial class m_WebSiteConfig:EntityBase
    {
		
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? ConfigId { get; set; }
		
        /// <summary>
        /// 网站名称
        /// </summary>
        
        public string WebSiteName { get; set; }
		
        /// <summary>
        /// 网站地址
        /// </summary>
        
        public string WebSiteUrl { get; set; }
		
        /// <summary>
        /// 网站标题
        /// </summary>
        
        public string WebSiteTitle { get; set; }
		
        /// <summary>
        /// 网站关键字
        /// </summary>
        
        public string WebSiteKeyWord { get; set; }
		
        /// <summary>
        /// 网站描述
        /// </summary>
        
        public string WebSiteDescription { get; set; }
		
        /// <summary>
        /// 底部版权申明
        /// </summary>
        
        public string CopyrightText { get; set; }
		
        /// <summary>
        /// 是否开放登录
        /// </summary>
        
        public bool? IsLogin { get; set; }
		
        /// <summary>
        /// 是否开放注册
        /// </summary>
        
        public bool? IsRegister { get; set; }
        /// <summary>
        /// 网站备案号
        /// </summary>
        public string FilingNo { get; set; }


    }
}