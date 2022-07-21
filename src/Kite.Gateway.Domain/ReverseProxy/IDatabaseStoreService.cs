using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kite.Gateway.Domain.ReverseProxy.Models;
namespace Kite.Gateway.Domain.ReverseProxy
{
    public interface IDatabaseStoreService
    {
        /// <summary>
        /// 获取加载数据
        /// </summary>
        /// <returns></returns>
        Task<List<YarpDataModel>> GetServiceConfig();
    }
}
