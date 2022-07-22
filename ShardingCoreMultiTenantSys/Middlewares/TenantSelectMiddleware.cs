using ShardingCore.Core.VirtualDatabase.VirtualDataSources.Abstractions;
using ShardingCoreMultiTenantSys.DbContexts;
using ShardingCoreMultiTenantSys.Tenants;

namespace ShardingCoreMultiTenantSys.Middlewares
{
    public class TenantSelectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenantManager _tenantManager;

        public TenantSelectMiddleware(RequestDelegate next,ITenantManager tenantManager)
        {
            _next = next;
            _tenantManager = tenantManager;
        }

        /// <summary>
        /// 1.中间件的方法必须叫Invoke，且为public，非static。
        /// 2.Invoke方法第一个参数必须是HttpContext类型。
        /// 3.Invoke方法必须返回Task。
        /// 4.Invoke方法可以有多个参数，除HttpContext外其它参数会尝试从依赖注入容器中获取。
        /// 5.Invoke方法不能有重载。
        /// </summary>
        /// Author : Napoleon
        /// Created : 2020/1/30 21:30
        public async Task Invoke(HttpContext context)
        {

            if (context.Request.Path.ToString().StartsWith("/api/tenant", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    await _next(context);
                    return;
                }

                var tenantId = context.User.Claims.FirstOrDefault((o) => o.Type == "uid")?.Value;
                if (string.IsNullOrWhiteSpace(tenantId))
                {
                    await DoUnAuthorized(context, "not found tenant id");
                    return;
                }

                using (_tenantManager.CreateScope(tenantId))
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task DoUnAuthorized(HttpContext context, string msg)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(msg);
        }
    }
}
