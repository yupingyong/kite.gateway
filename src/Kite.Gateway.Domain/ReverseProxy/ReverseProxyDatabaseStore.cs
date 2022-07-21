using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Health;
using Yarp.ReverseProxy.LoadBalancing;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public class ReverseProxyDatabaseStore : IReverseProxyDatabaseStore
    {
        private InDatabaseReloadToken _reloadToken=new InDatabaseReloadToken();
        private IDatabaseStoreService _databaseStoreService;
        public ReverseProxyDatabaseStore(IDatabaseStoreService databaseStoreService)
        {
            _databaseStoreService = databaseStoreService;
        }
        public async Task<IProxyConfig> GetConfig()
        {
            var serviceConfigs =await _databaseStoreService.GetServiceConfig();
            var routeConfigs = new List<RouteConfig>();
            var clusterConfigs = new List<ClusterConfig>();
            //处理配置项
            foreach (var cfg in serviceConfigs)
            {
                var transforms = new List<Dictionary<string, string>>();
                var transformData= new Dictionary<string, string>();
                foreach (var transform in cfg.RouteTransforms)
                {
                    transformData.Add(transform.TransformsName, transform.TransformsValue);
                }
                transforms.Add(transformData);
                //路由配置
                routeConfigs.Add(new RouteConfig()
                {
                    RouteId = cfg.Route.RouteId,
                    ClusterId = cfg.Route.RouteId,
                    Match = new RouteMatch()
                    {
                        Path = cfg.Route.RouteMatchPath
                    },
                    Transforms = transforms,
                });
                //集群配置
                //集群目的地配置数据
                var destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase);
                foreach (var item in cfg.ClusterDestinations)
                {
                    destinations.Add(item.DestinationName, new DestinationConfig() 
                    { 
                        Address = item.DestinationAddress
                    });
                }
                //健康检查配置数据
                HealthCheckConfig healthCheck = null;
                if (cfg.ClusterHealthCheck != null)
                {
                    healthCheck = new HealthCheckConfig() 
                    {
                        Active=new ActiveHealthCheckConfig()
                        {
                            Enabled = cfg.ClusterHealthCheck.Enabled,
                            Interval = TimeSpan.FromSeconds(cfg.ClusterHealthCheck.Interval),
                            Timeout = TimeSpan.FromSeconds(cfg.ClusterHealthCheck.Timeout),
                            Path = cfg.ClusterHealthCheck.Path,
                            Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures
                        }
                    };
                }
                //
                clusterConfigs.Add(new ClusterConfig()
                {
                    ClusterId = cfg.Route.RouteId,
                    Destinations = destinations,
                    LoadBalancingPolicy = cfg.Cluster.LoadBalancingPolicy,
                    HealthCheck= healthCheck
                });
            }
            //
            var proxyConfig = new InDatabaseStoreConfig(routeConfigs, clusterConfigs);
            return proxyConfig;
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
    }

        public void Reload()
        {
            Interlocked.Exchange(ref this._reloadToken,
                new InDatabaseReloadToken()).OnReload();
        }
    }
}
