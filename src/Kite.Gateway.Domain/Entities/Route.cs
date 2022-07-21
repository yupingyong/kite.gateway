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
    /// 服务配置表
    /// </summary>
    public class Route : Entity<Guid>
    {
        public Route() { }
        public Route(Guid id) : base(id)
        {
        }
        /// <summary>
        /// 路由ID同主键ID
        /// </summary>
        [MaxLength(64)]
        public string RouteId { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        [MaxLength(64)]
        public string RouteName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(512)]
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool UseState { get; set; }

        /// <summary>
        /// 路由路径规则
        /// </summary>
        [MaxLength(128)]
        public string RouteMatchPath { get; set; }

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
