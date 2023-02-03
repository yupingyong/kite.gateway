using Kite.Gateway.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.Domain.Entities
{
    /// <summary>
    /// 中间件信息记录表
    /// </summary>
    public class Middleware : Entity<int>
    {
        /// <summary>
        /// 中间件名称
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; }
        /// <summary>
        /// 服务端地址
        /// </summary>
        [MaxLength(1024)]
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
        [MaxLength(1024)]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}
