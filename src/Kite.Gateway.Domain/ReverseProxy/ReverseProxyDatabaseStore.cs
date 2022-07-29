using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Health;
using Microsoft.Extensions.Options;
using Kite.Gateway.Domain.Shared.Options;
using Volo.Abp.DependencyInjection;

namespace Kite.Gateway.Domain.ReverseProxy
{
    public class ReverseProxyDatabaseStore : ITransientDependency, IReverseProxyDatabaseStore
    {
        private InDatabaseReloadToken _reloadToken=new InDatabaseReloadToken();
        private YarpOption _yarpOption;
        public ReverseProxyDatabaseStore(IOptions<YarpOption> options)
        {
            _yarpOption = options.Value;
        }
        public InDatabaseStoreConfig GetConfig()
        {
            //
            var routeConfigs = new List<RouteConfig>();
            var clusterConfigs = new List<ClusterConfig>();
            //处理配置项
            foreach (var route in _yarpOption.Routes)
            {
                var transforms = new List<Dictionary<string, string>>();
                var transformData = new Dictionary<string, string>();
                foreach (var transform in route.RouteTransforms)
                {
                    transformData.Add(transform.TransformsName, transform.TransformsValue);
                }
                transforms.Add(transformData);
                //路由配置
                routeConfigs.Add(new RouteConfig()
                {
                    RouteId = route.RouteId,
                    ClusterId = route.RouteId,
                    Match = new RouteMatch()
                    {
                        Path = route.RouteMatchPath
                    },
                    Transforms = transforms,
                });
                //集群配置
                //集群目的地配置数据
                var destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase);
                foreach (var item in route.Cluster.ClusterDestinations)
                {
                    destinations.Add(item.DestinationName, new DestinationConfig()
                    {
                        Address = item.DestinationAddress
                    });
                }
                //健康检查配置数据
                HealthCheckConfig healthCheck = null;
                if (route.Cluster.ClusterHealthCheck != null)
                {
                    healthCheck = new HealthCheckConfig()
                    {
                        Active = new ActiveHealthCheckConfig()
                        {
                            Enabled = route.Cluster.ClusterHealthCheck.Enabled,
                            Interval = TimeSpan.FromSeconds(route.Cluster.ClusterHealthCheck.Interval),
                            Timeout = TimeSpan.FromSeconds(route.Cluster.ClusterHealthCheck.Timeout),
                            Path = route.Cluster.ClusterHealthCheck.Path,
                            Policy = HealthCheckConstants.ActivePolicy.ConsecutiveFailures
                        }
                    };
                }
                //
                clusterConfigs.Add(new ClusterConfig()
                {
                    ClusterId = route.RouteId,
                    Destinations = destinations,
                    LoadBalancingPolicy = route.Cluster.LoadBalancingPolicy,
                    HealthCheck = healthCheck
                });
            }
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
