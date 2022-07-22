using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Mods;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.TenantSys.Shardings;

public class OrderModTableRoute:AbstractSimpleShardingModKeyStringVirtualTableRoute<Order>
{
    public OrderModTableRoute() : base(2, 5)
    {
    }

    public override void Configure(EntityMetadataTableBuilder<Order> builder)
    {
        builder.ShardingProperty(o => o.Id);
    }
}