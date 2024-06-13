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
    public class AuthMenuDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 授权码(路由地址/按钮标识)
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 所属上级菜单
        /// </summary>

        public Guid ParentId { get; set; }

        /// <summary>
        /// 是否菜单
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
