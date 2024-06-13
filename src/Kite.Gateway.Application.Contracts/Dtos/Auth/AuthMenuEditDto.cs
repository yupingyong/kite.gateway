using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthMenuEditDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// 授权码(路由地址/按钮标识)
        /// </summary>
        [Required]
        public string AuthCode { get; set; }

        /// <summary>
        /// 授权码(路由地址/按钮标识)
        /// </summary>
        [Required]
        public string MenuPath { get; set; }

        /// <summary>
        /// 所属上级菜单
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        ///// <summary>
        ///// 是否菜单
        ///// </summary>
        //[Required]
        //public bool IsMenu { get; set; }
    }
}
