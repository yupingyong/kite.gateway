using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kite.Gateway.Domain.Entities.Auth
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class AuthRole : Entity<Guid>, ISoftDelete
    {
        public AuthRole() { }
        public AuthRole(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 角色名
        /// </summary>
        [MaxLength(128)]
        public string RoleName { get; set; }
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
