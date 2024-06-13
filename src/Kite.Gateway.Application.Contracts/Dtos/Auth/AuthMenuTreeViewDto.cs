using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Dtos.Auth
{
    /// <summary>
    /// 权限设置时树菜单
    /// </summary>
    public class AuthMenuTreeViewDto
    {
        /// <summary>
        /// 节点唯一索引值，用于对指定节点进行各类操作
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 节点标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 节点字段名
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 点击节点弹出新窗口对应的 url。需开启 isJump 基础属性才有效。
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// 节点是否初始展开
        /// </summary>
        public bool Spread { get; set; }
        /// <summary>
        /// 节点是否初始为选中状态。需开启 showCheckbox 基础属性时有效。
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// 节点是否为禁用状态
        /// </summary>
        public bool Disabled { get; set; }
        /// <summary>
        /// 子节点。支持设定属性选项同父节点
        /// </summary>
        public List<AuthMenuTreeViewDto> Children { get; set; }
    }
}
