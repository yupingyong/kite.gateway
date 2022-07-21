using Kite.Gateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Kite.Gateway.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class KiteDbContext : AbpDbContext<KiteDbContext>
    {

        #region Entities from the modules
        public DbSet<ServiceGovernanceConfigure> ServiceGovernanceConfigures { get; set; }
        public DbSet<AuthenticationConfigure> AuthenticationConfigures { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Middleware> Middlewares { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<ClusterHealthCheck> ClusterHealthChecks { get; set; }
        public DbSet<RouteTransform> RouteTransforms { get; set; }
        public DbSet<ClusterDestination> ClusterDestinations { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Whitelist> Whitelists { get; set; }
        #endregion

        public KiteDbContext(DbContextOptions<KiteDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
