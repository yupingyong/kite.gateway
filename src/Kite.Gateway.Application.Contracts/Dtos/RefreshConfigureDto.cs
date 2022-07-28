using Kite.Gateway.Domain.Shared.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos
{
    public class RefreshConfigureDto
    {
        /// <summary>
        /// 身份认证数据
        /// </summary>
        [ValidateNever]
        public AuthenticationOption Authentication { get; set; }
        /// <summary>
        /// 中间件数据
        /// </summary>
        [ValidateNever]
        public List<WhitelistOption> Whitelists { get; set; }
        /// <summary>
        /// 服务治理数据
        /// </summary>
        [ValidateNever]
        public ServiceGovernanceOption ServiceGovernance { get; set; }
        /// <summary>
        /// 中间件数据
        /// </summary>
        [ValidateNever]
        public List<MiddlewareOption> Middlewares { get; set; }
        /// <summary>
        /// Yarp反向代理配置数据
        /// </summary>
        [ValidateNever]
        public YarpOption Yarp { get; set; }
    }
}
