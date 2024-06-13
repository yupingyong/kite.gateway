using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.Domain.Entities.Auth
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class AuthAccount : Entity<Guid>, ISoftDelete
    {
        public AuthAccount() { }
        public AuthAccount(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 账号名
        /// </summary>
        [MaxLength(32)]
        public string AccountName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(128)]
        public string NickName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        [MaxLength(512)]
        public string HeadUrl { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [MaxLength(128)]
        public string Password { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 所属角色
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// 是否软删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
