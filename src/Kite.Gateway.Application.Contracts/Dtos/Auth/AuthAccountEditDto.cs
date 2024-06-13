using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Kite.Gateway.Application.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthAccountEditDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 管理员这账号
        /// </summary>
        [Required]
        public string AccountName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [ValidateNever]
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Required]
        public string HeadUrl { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnable { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }
    }
}
