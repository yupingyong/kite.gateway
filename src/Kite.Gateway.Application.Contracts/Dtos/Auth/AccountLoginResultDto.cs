using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountLoginResultDto
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 管理员这账号
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>

        public bool IsUse { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>

        public Guid RoleId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
