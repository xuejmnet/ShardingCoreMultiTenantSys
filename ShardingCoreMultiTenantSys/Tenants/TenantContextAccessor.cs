namespace ShardingCoreMultiTenantSys.Tenants
{
    
    public class TenantContextAccessor:ITenantContextAccessor
    {
        private static readonly AsyncLocal<TenantContext?> _tenantContext = new AsyncLocal<TenantContext?>();
        public TenantContext? TenantContext 
        {
            get => _tenantContext.Value;
            set => _tenantContext.Value = value;
        }

    }
}
