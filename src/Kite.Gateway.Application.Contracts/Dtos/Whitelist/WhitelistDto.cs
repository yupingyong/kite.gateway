using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.Whitelist
{
    public class WhitelistDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 所属路由(为空则全局)
        /// </summary>
        public Guid? RouteId { get; set; }
        /// <summary>
        /// 白名单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 过滤类型(0.路径过滤 1.正则过滤)
        /// </summary>
        public FilterTypeEnum FilterType { get; set; }
        /// <summary>
        /// 过滤文本(根据类型保存的值为路径或者正则表达式)
        /// </summary>
        public string FilterText { get; set; }
        /// <summary>
        /// 请求方式(多请求方式使用,分隔)
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public bool UseState { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 关联路由名称
        /// </summary>
        public string RouteName { get; set; }
    }
}
