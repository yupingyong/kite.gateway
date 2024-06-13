using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using System.ComponentModel.DataAnnotations;
using Kite.Gateway.Domain.Shared.Enums.Auth;

namespace Kite.Gateway.Domain.Entities.Auth
{
    public class AuthMenu : Entity<Guid>, ISoftDelete
    {
        public AuthMenu() { }
        public AuthMenu(Guid id) 
        {
            Id = id;
        }
        /// <summary>
        /// 菜单名
        /// </summary>
        [MaxLength(128)]
        public string MenuName { get; set; }
        /// <summary>
        /// 授权码(路由地址/按钮标识)
        /// </summary>
        [MaxLength(256)]
        public string AuthCode { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum MenuType { get; set; }
        /// <summary>
        /// 所属上级菜单
        /// </summary>
        public Guid ParentId { get; set; }
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
