﻿using Kite.Gateway.Application.Contracts.Dtos;
using Kite.Gateway.Application.Contracts.Dtos.Node;
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
        /// 刷新配置数据
        /// </summary>
        /// <param name="refreshConfigure"></param>
        /// <returns></returns>
        Task<KiteResult> RefreshConfigureAsync(RefreshConfigureDto refreshConfigure);
    }
}
