using System.Collections.Concurrent;
using ShardingCore.Core.RuntimeContexts;

namespace ShardingCoreMultiTenantSys.Tenants
{
    public class DefaultTenantManager:ITenantManager
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;
        private readonly ConcurrentDictionary<string, IShardingRuntimeContext> _cache = new();

        public DefaultTenantManager(ITenantContextAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
        }

        public List<string> GetAll()
        {
            return _cache.Keys.ToList();
        }

        public TenantContext GetCurrentTenantContext()
        {
            return _tenantContextAccessor.TenantContext;
        }

        public bool AddTenantSharding(string tenantId, IShardingRuntimeContext shardingRuntimeContext)
        {
            return _cache.TryAdd(tenantId, shardingRuntimeContext);
        }

        public TenantScope CreateScope(string tenantId)
        {
            if (!_cache.TryGetValue(tenantId, out var shardingRuntimeContext))
            {
                throw new InvalidOperationException("未找到对应租户的配置");
            }

            _tenantContextAccessor.TenantContext = new TenantContext(shardingRuntimeContext);
            return new TenantScope(_tenantContextAccessor);
        }
    }
}
