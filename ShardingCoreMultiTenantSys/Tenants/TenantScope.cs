namespace ShardingCoreMultiTenantSys.Tenants
{
    public class TenantScope:IDisposable
    {
        public TenantScope(ITenantContextAccessor tenantContextAccessor)
        {
            TenantContextAccessor = tenantContextAccessor;
        }

        public ITenantContextAccessor TenantContextAccessor { get; }

        public void Dispose()
        {
        }
    }
    
}
