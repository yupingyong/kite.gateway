using Kite.Gateway.Application.Contracts.Dtos.ServiceGovernance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts
{
    /// <summary>
    /// 服务治理相关配置
    /// </summary>
    public interface IServiceGovernanceAppService
    {
        /// <summary>
        /// 获取服务治理配置信息
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseResult<ServiceGovernanceConfigureDto>> GetServiceGovernanceConfigureAsync();
        /// <summary>
        /// 保存服务治理相关配置信息
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        Task<HttpResponseResult> SaveServiceGovernanceConfigureAsync(ServiceGovernanceConfigureDto configure);
    }
}
