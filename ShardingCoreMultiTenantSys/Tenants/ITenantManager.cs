using ShardingCore.Core.RuntimeContexts;

namespace ShardingCoreMultiTenantSys.Tenants
{
    public interface ITenantManager
    {
        /// <summary>
        /// 获取所有的租户
        /// </summary>
        /// <returns></returns>
        List<string> GetAll();
        /// <summary>
        /// 获取当前租户
        /// </summary>
        /// <returns></returns>
        TenantContext GetCurrentTenantContext();
        /// <summary>
        /// 添加租户信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="shardingRuntimeContext"></param>
        /// <returns></returns>
        bool AddTenantSharding(string tenantId, IShardingRuntimeContext shardingRuntimeContext);

        /// <summary>
        /// 创建租户环境
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        TenantScope CreateScope(string tenantId);
    }
}
