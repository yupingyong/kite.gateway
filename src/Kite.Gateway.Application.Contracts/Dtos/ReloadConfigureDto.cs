using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Kite.Gateway.Application.Contracts.Dtos
{
    public class ReloadConfigureDto
    {
        /// <summary>
        /// 是否重新加载身份认证数据
        /// </summary>
        [Required]
        public bool IsReloadAuthentication { get; set; }
        /// <summary>
        /// 是否重新加载白名单数据
        /// </summary>
        [Required]
        public bool IsReloadWhitelist { get; set; }
        /// <summary>
        /// 是否重新加载服务治理数据
        /// </summary>
        [Required]
        public bool IsReloadServiceGovernance { get; set; }
        /// <summary>
        /// 是否重新加载中间件数据
        /// </summary>
        [Required]
        public bool IsReloadMiddleware { get; set; }
        /// <summary>
        /// 是否重新加载Yarp相关数据
        /// </summary>
        [Required]
        public bool IsReloadYarp { get; set; }
    }
}
