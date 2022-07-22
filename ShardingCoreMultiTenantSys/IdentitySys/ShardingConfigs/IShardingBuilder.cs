using ShardingCore.Core.RuntimeContexts;

namespace ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;

public interface IShardingBuilder
{
    IShardingRuntimeContext Build(ShardingTenantOptions tenantOptions);
}