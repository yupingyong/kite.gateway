using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.Domain.Entities
{
    public class Administrator : Entity<Guid>
    {
        public Administrator() { }
        public Administrator(Guid id) : base(id)
        {
        }
        /// <summary>
        /// 管理员名
        /// </summary>
        [MaxLength(32)]
        public string AdminName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [MaxLength(64)]
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(64)]
        public string NickName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}
