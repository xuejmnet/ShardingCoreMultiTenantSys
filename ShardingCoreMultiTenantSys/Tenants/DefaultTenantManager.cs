using ShardingCore.Core.RuntimeContexts;

namespace ShardingCoreMultiTenantSys.Tenants
{
    public class DefaultTenantManager:ITenantManager
    {
        public IShardingRuntimeContext GetShardingRuntimeContext()
        {
            throw new NotImplementedException();
        }
    }
}
