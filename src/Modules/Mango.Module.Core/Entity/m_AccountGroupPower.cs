using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Core.Entity
{
    public partial class m_AccountGroupPower:EntityBase
    {
		
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? PId { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public int? MenuId { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public int? GroupId { get; set; }
		
    }
}