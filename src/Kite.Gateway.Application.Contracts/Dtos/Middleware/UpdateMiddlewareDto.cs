using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.Middleware
{
    public class UpdateMiddlewareDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 中间件名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 服务端地址
        /// </summary>
        [Required]
        public string Server { get; set; }
        /// <summary>
        /// 通信类型
        /// </summary>
        [Required]
        public SignalTypeEnum SignalType { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        [Required]
        public bool UseState { get; set; }
        /// <summary>
        /// 执行权重(数字越大越靠前)
        /// </summary>
        [Required]
        public int ExecWeight { get; set; }
        /// <summary>
        /// 中间件描述
        /// </summary>
        [StringLength(1024)]
        public string Description { get; set; }
    }
}
