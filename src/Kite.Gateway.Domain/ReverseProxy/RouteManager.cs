using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Mapster;
using Volo.Abp.Guids;
using Kite.Gateway.Domain.Entities;

namespace Kite.Gateway.Domain.ReverseProxy
{
    internal class RouteManager: DomainService,IRouteManager
    {
        private readonly IRepository<Route> _routeRepository;
        public RouteManager(IRepository<Route> routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<Route> CreateAsync(string routeName, string routeMatchPath, bool useState, string description)
        {
            var route = await _routeRepository.FirstOrDefaultAsync(x => x.RouteName == routeName || x.RouteMatchPath == routeMatchPath);
            if (route != null)
            {
                throw new Exception("路由名称或者路由路径已经存在");
            }
            var id = GuidGenerator.Create();
            return new Route(id) 
            {
                Created=DateTime.Now,
                RouteMatchPath = routeMatchPath,
                UseState = useState,
                RouteName = routeName,
                Updated=DateTime.Now,
                Description=string.IsNullOrEmpty(description)?"":description,
                RouteId= id.ToString(),
            };
        }

        public Task<RouteTransform> CreateRouteTransformAsync(Guid routeId, string transformsName, string transformsValue)
        {
            return Task.Run(() => 
            {
                return new RouteTransform(GuidGenerator.Create()) 
                {
                    RouteId = routeId,
                    TransformsName = transformsName,
                    TransformsValue = transformsValue
                };
            });
        }
    }
}
