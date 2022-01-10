using ShardingCore.Sharding.ReadWriteConfigurations;

namespace ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs
{
    public class ShardingTenantOptions
    {
        public  string ConfigId { get; set;}
        public  int Priority { get; set;}
        public  string DefaultDataSourceName { get; set;}
        public  string DefaultConnectionString { get; set; }
        public DbTypeEnum DbType { get; set; }
    }
}
