using Kite.Gateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public interface IRouteManager
    {
        /// <summary>
        /// 创建路由 
        /// </summary>
        /// <param name="routeName">路由名称</param>
        /// <param name="routeMatchPath">路由路径</param>
        /// <param name="useState">服务状态</param>
        /// <param name="description">路由描述</param>
        /// <returns></returns>
        Task<Route> CreateAsync(string routeName,string routeMatchPath, bool useState, string description);
        /// <summary>
        /// 创建路由交换
        /// </summary>
        /// <param name="routeId">路由ID</param>
        /// <param name="transformsName">交换项名称</param>
        /// <param name="transformsValue">交换项值</param>
        /// <returns></returns>
        Task<RouteTransform> CreateRouteTransformAsync(Guid routeId,string transformsName,string transformsValue);
    }
}
