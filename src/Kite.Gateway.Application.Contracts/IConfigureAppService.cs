using Kite.Gateway.Application.Contracts.Dtos;
using Kite.Gateway.Application.Contracts.Dtos.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    public interface IConfigureAppService : IApplicationService
    {
        /// <summary>
        /// 根据推送配置获取配置项数据
        /// </summary>
        /// <param name="reloadConfigure"></param>
        /// <returns></returns>
        Task<KiteResult<RefreshConfigureDto>> GetConfigureAsync(ReloadConfigureDto reloadConfigure);
    }
}
