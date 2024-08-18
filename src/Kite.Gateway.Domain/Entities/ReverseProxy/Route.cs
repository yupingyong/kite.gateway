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
        /// 所属服务ID
        /// </summary>
        public Guid ServiceId { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        [MaxLength(64)]
        public string RouteName { get; set; }
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
