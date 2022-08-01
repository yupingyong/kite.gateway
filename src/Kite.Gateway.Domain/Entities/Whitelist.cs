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
    /// 白名单
    /// </summary>
    public class Whitelist : Entity<Guid>
    {
        public Whitelist() { }
        public Whitelist(Guid id) : base(id)
        {
        }
        /// <summary>
        /// 所属路由(为空则全局)
        /// </summary>
        public Guid? RouteId { get; set; }
        /// <summary>
        /// 白名单名称
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; }
        /// <summary>
        /// 过滤文本(* 号则表示过滤所有)
        /// </summary>
        [MaxLength(128)]
        public string FilterText { get; set; }
        /// <summary>
        /// 请求方式(多请求方式使用,分隔)
        /// </summary>
        [MaxLength(64)]
        public string RequestMethod { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        public bool UseState { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        
    }
}
