using ShardingCore.Sharding.ReadWriteConfigurations;

namespace ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs
{
    public class ShardingTenantOptions
    {
        /// <summary>
        /// 默认数据源名称
        /// </summary>
        public  string DefaultDataSourceName { get; set;}
        /// <summary>
        /// 默认数据库地址
        /// </summary>
        public  string DefaultConnectionString { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbTypeEnum DbType { get; set; }
        /// <summary>
        /// 分片模式 取模还是按月
        /// </summary>
        public OrderShardingTypeEnum OrderShardingType { get; set; }
        /// <summary>
        /// 按月分片其实时间
        /// </summary>
        public DateTime BeginTimeForSharding { get; set; }
        /// <summary>
        /// 分片迁移的命名空间
        /// </summary>
        public string MigrationNamespace { get; set; }
    }
}
