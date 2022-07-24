using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Mods;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.TenantSys.Shardings;

public class OrderModTableRoute:AbstractSimpleShardingModKeyStringVirtualTableRoute<Order>
{
    private readonly ShardingTenantOptions _shardingTenantOptions;

    public OrderModTableRoute(ShardingTenantOptions shardingTenantOptions) : base(2, 5)
    {
        _shardingTenantOptions = shardingTenantOptions;
    }

    public override void Configure(EntityMetadataTableBuilder<Order> builder)
    {
        builder.ShardingProperty(o => o.Id);
    }
}