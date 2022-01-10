using Microsoft.EntityFrameworkCore;
using ShardingCore.Core.VirtualRoutes.TableRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding;
using ShardingCore.Sharding.Abstractions;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Maps;

namespace ShardingCoreMultiTenantSys.DbContexts
{
    public class TenantDbContext:AbstractShardingDbContext,IShardingTableDbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OrderMap());
        }

        public IRouteTail RouteTail { get; set; }
    }
}
