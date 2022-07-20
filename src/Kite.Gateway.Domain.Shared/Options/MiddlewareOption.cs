using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Options
{
    /// <summary>
    /// 中间件配置项
    /// </summary>
    public class MiddlewareOption
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 中间件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务端地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 通信类型
        /// </summary>
        public SignalTypeEnum SignalType { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public bool UseState { get; set; }
        /// <summary>
        /// 执行权重(数字越大越靠前)
        /// </summary>
        public int ExecWeight { get; set; }
        /// <summary>
        /// 中间件描述
        /// </summary>
        public string Description { get; set; }
    }
}
