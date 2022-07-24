using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Abstractions;
using ShardingCore.VirtualRoutes.Mods;
using ShardingCore.VirtualRoutes.Months;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.TenantSys.Shardings
{
    public class OrderMonthTableRoute:AbstractSimpleShardingMonthKeyDateTimeVirtualTableRoute<Order>
    {
        private readonly ShardingTenantOptions _shardingTenantOptions;

        public OrderMonthTableRoute(ShardingTenantOptions shardingTenantOptions)
        {
            _shardingTenantOptions = shardingTenantOptions;
        }
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
            return _shardingTenantOptions.BeginTimeForSharding;
        }
    }
}
