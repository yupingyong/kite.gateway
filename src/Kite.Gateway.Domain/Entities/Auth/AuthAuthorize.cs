using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using System.ComponentModel.DataAnnotations;

namespace Kite.Gateway.Domain.Entities.Auth
{
    /// <summary>
    /// 授权信息表
    /// </summary>
    public class AuthAuthorize : Entity<Guid>, ISoftDelete
    {
        public AuthAuthorize() { }
        public AuthAuthorize(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 是否软删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
