using Kite.Gateway.Application.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Kite.Gateway.Application.Contracts
{
    public interface IRefreshAppService: IApplicationService
    {
        /// <summary>
        /// 重新加载配置数据
        /// </summary>
        /// <param name="reloadConfigure"></param>
        /// <returns></returns>
        Task<HttpResponseResult> ReloadConfigureAsync(ReloadConfigureDto reloadConfigure);
    }
}
