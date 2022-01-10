namespace ShardingCoreMultiTenantSys.TenantSys.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
