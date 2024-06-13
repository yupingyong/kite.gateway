using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Services.Auth.Models
{
    /// <summary>
    /// 系统默认账号
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 账号名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadUrl { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
