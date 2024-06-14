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
    /// 服务交换配置表
    /// </summary>
    public class RouteTransform : Entity<Guid>
    {
        public RouteTransform() { }
        public RouteTransform(Guid id) : base(id)
        {
        }
        /// <summary>
        /// 关联路由ID
        /// </summary>
        public Guid RouteId { get; set; }
        /// <summary>
        /// 交换配置项名称
        /// </summary>
        [MaxLength(128)]
        public string TransformsName { get; set; }
        /// <summary>
        /// 交换配置项值
        /// </summary>
        [MaxLength(128)]
        public string TransformsValue { get; set; }
    }
}
