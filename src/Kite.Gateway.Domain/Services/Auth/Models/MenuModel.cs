using Kite.Gateway.Domain.Shared.Enums;
using Kite.Gateway.Domain.Shared.Enums.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Services.Auth.Models
{
    /// <summary>
    /// 授权菜单模型
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// 所属上级菜单
        /// </summary>
        public Guid ParentId { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum MenuType { get; set; }
        /// <summary>
        /// 所属子集
        /// </summary>
        public List<MenuModel> Children { get; set; }
    }
}
