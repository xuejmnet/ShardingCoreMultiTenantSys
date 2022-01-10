namespace ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities
{
    public class SysUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
