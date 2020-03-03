using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Core.Entity
{
    public partial class m_AccountGroupMenu:EntityBase
    {
		
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? MenuId { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public string MenuName { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public string AreaName { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public string ControllerName { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public string ActionName { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public int? ModuleId { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public string ParentCode { get; set; }
		
    }
}