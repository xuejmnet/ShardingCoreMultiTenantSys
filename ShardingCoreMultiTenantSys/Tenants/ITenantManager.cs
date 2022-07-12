using ShardingCore.Core.RuntimeContexts;

namespace ShardingCoreMultiTenantSys.Tenants
{
    public interface ITenantManager
    {
        IShardingRuntimeContext GetShardingRuntimeContext();
        void AddTenantSharding(string tenantId, IShardingRuntimeContext shardingRuntimeContext);
    }
}
