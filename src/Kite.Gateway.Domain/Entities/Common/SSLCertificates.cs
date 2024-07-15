using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Kite.Gateway.Domain.Entities.Common
{
    /// <summary>
    /// SSL证书信息表
    /// </summary>
    public class SSLCertificates : Entity<Guid>, ISoftDelete
    {
        public SSLCertificates() { }
        public SSLCertificates(Guid id) : base(id)
        {
        }
        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(256)]
        public string Title { get; set; }
        /// <summary>
        /// 证书路径
        /// </summary>
        [MaxLength(256)]
        public string Path { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        [MaxLength(128)]
        public string Password { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public DateOnly ExpirationDate { get; set; }
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
