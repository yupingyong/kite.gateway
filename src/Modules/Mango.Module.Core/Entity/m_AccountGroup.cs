using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Mango.Framework.Data;
namespace Mango.Module.Core.Entity
{
    public partial class m_AccountGroup:EntityBase
    {
		
        /// <summary>
        /// 用户组Id
        /// </summary>
        [Key]
        public int? GroupId { get; set; }
		
        /// <summary>
        /// 用户组名称
        /// </summary>
        
        public string GroupName { get; set; }
		
        /// <summary>
        /// 
        /// </summary>
        
        public bool? IsDefault { get; set; }
		
    }
}