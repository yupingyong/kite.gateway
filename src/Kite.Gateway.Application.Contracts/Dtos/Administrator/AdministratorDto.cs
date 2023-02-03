using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos.Administrator
{
    public class AdministratorDto
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 管理员名
        /// </summary>
        public string AdminName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}
