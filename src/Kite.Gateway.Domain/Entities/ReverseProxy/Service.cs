using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.Domain.Entities.ReverseProxy
{
    public class Service : Entity<Guid>, ISoftDelete
    {
        public Service() { }
        public Service(Guid id) : base(id)
        {
        }
        /// <summary>
        /// 服务名
        /// </summary>
        [MaxLength(256)]
        public string ServiceName { get; set; }
        /// <summary>
        /// 关联证书
        /// </summary>
        public Guid SSLCertificateId { get; set; }
        /// <summary>
        /// 域名解析
        /// </summary>
        [MaxLength(2048)]
        public string Hosts { get; set; }
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
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// 是否软删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
