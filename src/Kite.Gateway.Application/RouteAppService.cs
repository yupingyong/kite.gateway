using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Mapster;
using Volo.Abp.Application.Services;
using Kite.Gateway.Domain;
using Microsoft.AspNetCore.Mvc;
using Kite.Gateway.Domain.ReverseProxy;
using Volo.Abp;
using Kite.Gateway.Application.Contracts.Dtos.ReverseProxy;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Shared.Enums;

namespace Kite.Gateway.Application
{
    /// <summary>
    /// 路由相关接口
    /// </summary>
    public class RouteAppService: BaseApplicationService,IRouteAppService
    {
        private readonly IClusterManager _clusterManager;
        private readonly IRouteManager _routeManager;

        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<RouteTransform> _routeTransformRepository;
        private readonly IRepository<Cluster> _clusterRepository;
        private readonly IRepository<ClusterDestination> _clusterDestinationRepository;
        private readonly IRepository<ClusterHealthCheck> _clusterHealthCheckRepository;
        public RouteAppService(IRepository<Route> routeRepository
            , IRepository<Cluster> clusterRepository
            , IRepository<ClusterDestination> clusterDestinationRepository
            , IRepository<RouteTransform> routeTransformRepository
            , IClusterManager clusterManager, IRouteManager routeManager, IRepository<ClusterHealthCheck> clusterHealthCheckRepository)
        {
            _clusterManager = clusterManager;
            _routeManager = routeManager;

            _routeRepository = routeRepository;
            _clusterRepository = clusterRepository;
            _clusterDestinationRepository = clusterDestinationRepository;
            _routeTransformRepository = routeTransformRepository;
            _clusterHealthCheckRepository = clusterHealthCheckRepository;
        }
        /// <summary>
        /// 创建路由
        /// </summary>
        /// <param name="createRouteDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> CreateAsync(CreateRouteDto createRouteDto)
        {
            var route = await _routeManager.CreateAsync(createRouteDto.RouteName, createRouteDto.RouteMatchPath, createRouteDto.UseState, createRouteDto.Description);
            //插入路由信息
            await _routeRepository.InsertAsync(route);
            await CurrentUnitOfWork.SaveChangesAsync();
            //路由转换配置
            if (!string.IsNullOrEmpty(createRouteDto.PathRemovePrefix)&&createRouteDto.PathRemovePrefix!="")
            {
                await _routeTransformRepository.InsertAsync(await _routeManager.CreateRouteTransformAsync(route.Id, "PathRemovePrefix", createRouteDto.PathRemovePrefix));
            }
            if (!string.IsNullOrEmpty(createRouteDto.PathPrefix) && createRouteDto.PathPrefix != "")
            {
                await _routeTransformRepository.InsertAsync(await _routeManager.CreateRouteTransformAsync(route.Id, "PathPrefix", createRouteDto.PathPrefix));
            }
            //插入集群信息
            var cluster = await _clusterManager.CreateAsync(route.Id, route.RouteName
                , createRouteDto.ServiceGovernanceType
                , createRouteDto.ServiceGovernanceType != ServiceGovernanceType.Default ? createRouteDto.ClusterDestinationValue : ""
                , createRouteDto.LoadBalancingPolicy);

            await _clusterRepository.InsertAsync(cluster);
            await CurrentUnitOfWork.SaveChangesAsync();
            //集群目的地信息
            if (createRouteDto.ServiceGovernanceType == ServiceGovernanceType.Default)
            {
                var clusterDestinations = new List<ClusterDestination>();

                var destinations = createRouteDto.ClusterDestinationValue.Split(',');
                foreach (var destination in destinations)
                {
                    clusterDestinations.Add(await _clusterManager.CreateClusterDestinationAsync(cluster.Id, GuidGenerator.Create().ToString().Replace("-", ""), destination));
                }
                await _clusterDestinationRepository.InsertManyAsync(clusterDestinations);
            }
            //集群健康检查信息
            if (createRouteDto.ClusterHealthCheck != null)
            {
                createRouteDto.ClusterHealthCheck.ClusterId = cluster.Id;
                var healthCheck =await _clusterManager.CreateHealthCheckAsync(createRouteDto.ClusterHealthCheck);
                await _clusterHealthCheckRepository.InsertAsync(healthCheck);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// 删除路由
        /// </summary>
        /// <param name="routeId">路由ID</param>
        /// <returns></returns>
        public async Task<KiteResult> DeleteAsync(Guid routeId)
        {
            await _routeRepository.DeleteAsync(x=>x.Id==routeId);
            return Ok();
        }
        /// <summary>
        /// 获取路由列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public async Task<KitePageResult<List<RoutePageDto>>> GetPageAsync(string kw = "", int page = 1, int pageSize = 10)
        {
            var query =(await _routeRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrEmpty(kw) && kw != "", x => x.RouteName.Contains(kw));
            var totalCount = query.Count();
            var result = query
                .Join((await _clusterRepository.GetQueryableAsync()), x => x.Id, y => y.RouteId, (x, y) =>new RoutePageDto()
                {
                    Created = x.Created,
                    Description=x.Description,
                    Id = x.Id,
                    LoadBalancingPolicy=y.LoadBalancingPolicy,
                    RouteMatchPath=x.RouteMatchPath,
                    RouteName=x.RouteName,
                    ServiceGovernanceName=y.ServiceGovernanceName,
                    ServiceGovernanceType=y.ServiceGovernanceType,
                    UseState =x.UseState,
                    ClusterId=y.Id
                })
                .OrderByDescending(x => x.Created)
                .PageBy((page - 1) * pageSize, pageSize)
                .ToList();
            //获取集群目的地
            var clusters = await _clusterDestinationRepository.GetListAsync();
            foreach (var item in result)
            {
                if (item.ServiceGovernanceType == ServiceGovernanceType.Default)
                {
                    item.ServiceGovernanceName = string.Join(',', clusters.Where(x => x.ClusterId == item.ClusterId).Select(x => x.DestinationAddress).ToList());
                }
            }
            return Ok(result, totalCount);
        }
        /// <summary>
        /// 根据路由ID获取路由信息
        /// </summary>
        /// <param name="id">路由ID</param>
        /// <returns></returns>
        public async Task<KiteResult<RouteDto>> GetAsync(Guid id)
        {
            var query = await _routeRepository.GetQueryableAsync();
            //查询基本信息
            var route = query.ProjectToType<RouteDto>().Where(x => x.Id == id).FirstOrDefault();
            if (route == null)
            {
                ThrownFailed("路由数据不存在");
            }
            route.RouteTransforms = (await _routeTransformRepository.GetQueryableAsync()).ProjectToType<RouteTransformDto>().Where(x => x.RouteId == route.Id).ToList();

            //查询集群信息
            route.Cluster = (await _clusterRepository.GetQueryableAsync()).ProjectToType<ClusterDto>().Where(x => x.RouteId == route.Id).First();
            route.Cluster.ClusterDestinations = (await _clusterDestinationRepository.GetQueryableAsync()).ProjectToType<ClusterDestinationDto>().Where(x => x.ClusterId == route.Cluster.Id).ToList();
            route.Cluster.HealthCheck = (await _clusterHealthCheckRepository.GetQueryableAsync())
                .Where(x => x.ClusterId == route.Cluster.Id)
                .ProjectToType<ClusterHealthCheckDto>()
                .FirstOrDefault();
            return Ok(route);
        }
        /// <summary>
        /// 更新路由信息
        /// </summary>
        /// <param name="updateRouteDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> UpdateAsync(UpdateRouteDto updateRouteDto)
        {
            //更新路由信息
            var model = await _routeRepository.FirstOrDefaultAsync(x => x.Id == updateRouteDto.RouteId);
            model.Updated = DateTime.Now;
            TypeAdapter.Adapt(updateRouteDto, model);
            await _routeRepository.UpdateAsync(model);
            //路由转换配置
            await _routeTransformRepository.DeleteAsync(x => x.RouteId == model.Id);
            if (!string.IsNullOrEmpty(updateRouteDto.PathRemovePrefix) && updateRouteDto.PathRemovePrefix != "")
            {
                await _routeTransformRepository.InsertAsync(await _routeManager.CreateRouteTransformAsync(model.Id, "PathRemovePrefix", updateRouteDto.PathRemovePrefix));
            }
            if (!string.IsNullOrEmpty(updateRouteDto.PathPrefix) && updateRouteDto.PathPrefix != "")
            {
                await _routeTransformRepository.InsertAsync(await _routeManager.CreateRouteTransformAsync(model.Id, "PathPrefix", updateRouteDto.PathPrefix));
            }
            //更新集群信息
            var cluster = await _clusterRepository.FirstOrDefaultAsync(x => x.RouteId == updateRouteDto.RouteId);
            TypeAdapter.Adapt(updateRouteDto, cluster);
            if (updateRouteDto.ServiceGovernanceType != ServiceGovernanceType.Default)
            {
                cluster.ServiceGovernanceName = updateRouteDto.ClusterDestinationValue;
            }
            else
            {
                cluster.ServiceGovernanceName = "";
            }
            await _clusterRepository.UpdateAsync(cluster);

            var clusterDestinations = await _clusterDestinationRepository.GetListAsync(x=>x.ClusterId==cluster.Id);
            //移除原有集群下的目的地数据
            await _clusterDestinationRepository.DeleteManyAsync(clusterDestinations);
            //新增集群下目的地地址
            //集群目的地信息
            if (updateRouteDto.ServiceGovernanceType == ServiceGovernanceType.Default)
            {
                var newDestinations = new List<ClusterDestination>();

                var destinations = updateRouteDto.ClusterDestinationValue.Split(',');
                foreach (var destination in destinations)
                {
                    newDestinations.Add(await _clusterManager.CreateClusterDestinationAsync(cluster.Id, GuidGenerator.Create().ToString().Replace("-", ""), destination));
                }
                await _clusterDestinationRepository.InsertManyAsync(newDestinations);
            }
            //集群健康检查信息
            if (updateRouteDto.ClusterHealthCheck != null)
            {
                var healthCheck = await _clusterHealthCheckRepository.FirstOrDefaultAsync(x=>x.ClusterId==cluster.Id);
                await _clusterHealthCheckRepository.UpdateAsync(healthCheck);
            }
            
            return Ok();
        }
        /// <summary>
        /// 更新路由状态
        /// </summary>
        /// <param name="routeId">路由ID</param>
        /// <param name="useState">路由状态(1.开启 0.关闭)</param>
        /// <returns></returns>
        public async Task<KiteResult> UpdateStateAsync(Guid routeId, bool useState)
        {
            var model = await _routeRepository.FindAsync(x => x.Id == routeId);
            model.UseState = useState;
            model.Updated = DateTime.Now;
            await _routeRepository.UpdateAsync(model);

            return Ok();
        }

        public async Task<KiteResult<List<RouteMainDto>>> GetAsync()
        {
            var result = (await _routeRepository.GetQueryableAsync())
                .OrderByDescending(x => x.Created)
                .ProjectToType<RouteMainDto>()
                .ToList();
            return Ok(result);
        }
    }
}
