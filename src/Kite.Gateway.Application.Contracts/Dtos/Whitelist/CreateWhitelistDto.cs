using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.Whitelist
{
    public class CreateWhitelistDto
    {
        /// <summary>
        /// 所属路由(为空则全局)
        /// </summary>
        [Required]
        public Guid? RouteId { get; set; }
        /// <summary>
        /// 白名单名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 过滤类型(0.路径过滤 1.正则过滤)
        /// </summary>
        [Required]
        public FilterTypeEnum FilterType { get; set; }
        /// <summary>
        /// 过滤文本(根据类型保存的值为路径或者正则表达式)
        /// </summary>
        [Required]
        public string FilterText { get; set; }
        /// <summary>
        /// 请求方式(多请求方式使用,分隔)
        /// </summary>
        [Required]
        public string RequestMethod { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public bool UseState { get; set; }
    }
}
