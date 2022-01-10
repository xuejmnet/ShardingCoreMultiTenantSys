namespace ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities
{
    public class SysUserTenantConfig
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 添加ShardingCore配置的Json包
        /// </summary>
        public string ConfigJson { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
