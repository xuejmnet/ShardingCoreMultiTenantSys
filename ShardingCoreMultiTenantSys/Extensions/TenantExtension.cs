using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShardingCore.Extensions;
using ShardingCore.Helpers;
using ShardingCoreMultiTenantSys.DbContexts;
using ShardingCoreMultiTenantSys.IdentitySys;
using ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;
using ShardingCoreMultiTenantSys.Tenants;

namespace ShardingCoreMultiTenantSys.Extensions
{
    public static class TenantExtension
    {
        public static void InitTenant(this IServiceProvider serviceProvider)
        {
            var tenantManager = serviceProvider.GetRequiredService<ITenantManager>();
            var shardingBuilder = serviceProvider.GetRequiredService<IShardingBuilder>();
            
            using (var scope = serviceProvider.CreateScope())
            {
                var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                identityDbContext.Database.Migrate();
                var sysUserTenantConfigs = identityDbContext.Set<SysUserTenantConfig>().ToList();
                if (sysUserTenantConfigs.Any())
                {
                    foreach (var sysUserTenantConfig in sysUserTenantConfigs)
                    {
                        var shardingTenantOptions = JsonConvert.DeserializeObject<ShardingTenantOptions>(sysUserTenantConfig.ConfigJson);

                        var shardingRuntimeContext = shardingBuilder.Build(shardingTenantOptions);
                        
                        tenantManager.AddTenantSharding(sysUserTenantConfig.UserId, shardingRuntimeContext);
                    }
                }
            }

            var tenantIds = tenantManager.GetAll();
            foreach (var tenantId in tenantIds)
            {
                using(tenantManager.CreateScope(tenantId))
                using (var scope = serviceProvider.CreateScope())
                {
                    var shardingRuntimeContext = tenantManager.GetCurrentTenantContext().GetShardingRuntimeContext();
                    //开启定时任务
                    shardingRuntimeContext.UseAutoShardingCreate();
                    var tenantDbContext = scope.ServiceProvider.GetService<TenantDbContext>();
                    tenantDbContext.Database.Migrate();
                    //补偿表
                    shardingRuntimeContext.UseAutoTryCompensateTable();
                }
            }
        }
    }
}
