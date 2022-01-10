using Newtonsoft.Json;
using ShardingCore.Helpers;
using ShardingCoreMultiTenantSys.IdentitySys;
using ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;

namespace ShardingCoreMultiTenantSys.Extensions
{
    public static class TenantExtension
    {
        public static void InitTenant(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                identityDbContext.Database.EnsureCreated();
                var sysUserTenantConfigs = identityDbContext.Set<SysUserTenantConfig>().ToList();
                if (sysUserTenantConfigs.Any())
                {
                    foreach (var sysUserTenantConfig in sysUserTenantConfigs)
                    {
                        var shardingTenantOptions = JsonConvert.DeserializeObject<ShardingTenantOptions>(sysUserTenantConfig.ConfigJson);
                        DynamicShardingHelper.DynamicAppendVirtualDataSourceConfig(
                            new SqlShardingConfiguration(shardingTenantOptions));
                    }
                }
            }
        }
    }
}
