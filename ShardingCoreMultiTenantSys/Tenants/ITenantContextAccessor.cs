namespace ShardingCoreMultiTenantSys.Tenants
{
    public interface ITenantContextAccessor
    {
        TenantContext? TenantContext { get; set; }
    }
}
