using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Abstractions;
using ShardingCore.VirtualRoutes.Mods;
using ShardingCore.VirtualRoutes.Months;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.TenantSys.Shardings
{
    public class OrderVirtualTableRoute:AbstractSimpleShardingMonthKeyDateTimeVirtualTableRoute<Order>
    {
        public override void Configure(EntityMetadataTableBuilder<Order> builder)
        {
            builder.ShardingProperty(o => o.CreationTime);
        }

        public override bool AutoCreateTableByTime()
        {
            return true;

        }

        public override DateTime GetBeginTime()
        {
            return new DateTime(2022, 1, 1);
        }

        public override bool DoLogError => true;
    }
}
