using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShardingCore;
using ShardingCore.Bootstrappers;
using ShardingCoreMultiTenantSys.DbContexts;
using ShardingCoreMultiTenantSys.Extensions;
using ShardingCoreMultiTenantSys.IdentitySys;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;
using ShardingCoreMultiTenantSys.Middlewares;
using ShardingCoreMultiTenantSys.Tenants;
using ShardingCoreMultiTenantSys.TenantSys.Shardings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication();
#region 用户系统配置

builder.Services.AddDbContext<IdentityDbContext>(o =>
    o.UseSqlServer("Data Source=localhost;Initial Catalog=IdDb;Integrated Security=True;"));
//生成密钥
var keyByteArray = Encoding.ASCII.GetBytes("123123!@#!@#123123");
var signingKey = new SymmetricSecurityKey(keyByteArray);
//认证参数
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = "https://localhost:5000",
            ValidateAudience = true,
            ValidAudience = "api",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
        };
    });
#endregion

builder.Services.AddSingleton<ITenantManager, DefaultTenantManager>();
builder.Services.AddSingleton<ITenantContextAccessor, TenantContextAccessor>();
builder.Services.AddSingleton<IShardingBuilder, DefaultShardingBuilder>();
#region 配置ShardingCore

builder.Services.AddDbContext<TenantDbContext>((sp, b) =>
{
    var tenantManager = sp.GetRequiredService<ITenantManager>();
    var shardingRuntimeContext = tenantManager.GetCurrentTenantContext().GetShardingRuntimeContext();
    b.UseDefaultSharding<TenantDbContext>(shardingRuntimeContext);
});
#endregion

var app = builder.Build();

//初始化启动配置租户信息
app.Services.InitTenant();
app.UseAuthorization();

//在认证后启用租户选择中间件
app.UseMiddleware<TenantSelectMiddleware>();

app.MapControllers();

app.Run();
