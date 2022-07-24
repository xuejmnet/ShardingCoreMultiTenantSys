using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using ShardingCore;
using ShardingCore.Bootstrappers;
using ShardingCoreMultiTenantSys.DbContexts;
using ShardingCoreMultiTenantSys.Extensions;
using ShardingCoreMultiTenantSys.IdentitySys;
using ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;
using ShardingCoreMultiTenantSys.Middlewares;
using ShardingCoreMultiTenantSys.MigrationsAssemblies;
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

var provider = builder.Configuration.GetValue("Provider", "UnKnown");
//Add-Migration InitialCreate -Context TenantDbContext -OutputDir Migrations\SqlServer -Args "--provider SqlServer"
//Add-Migration InitialCreate -Context TenantDbContext -OutputDir Migrations\MySql -Args "--provider MySql"
builder.Services.AddDbContext<TenantDbContext>((sp, b) =>
{
    var tenantManager = sp.GetRequiredService<ITenantManager>();
    var currentTenantContext = tenantManager.GetCurrentTenantContext();
    //如果有上下文那么创建租户dbcontext否则就是启动命令Add-Migration
    if (currentTenantContext != null)
    {
        var shardingRuntimeContext = currentTenantContext.GetShardingRuntimeContext();
        b.UseDefaultSharding<TenantDbContext>(shardingRuntimeContext);
    }

    if (provider == "UnKnown")
    {
        throw new Exception("无法获取租户数据库信息");
    }

//命令启动时为了保证Add-Migration正常运行
    if (provider == "MySql")
    {
        b.UseMySql("server=127.0.0.1;port=3306;database=TenantDb;userid=root;password=L6yBtV6qNENrwBy7;",
                new MySqlServerVersion(new Version()))
            .UseMigrationNamespace(new MySqlMigrationNamespace())
            .ReplaceService<IMigrationsAssembly, MultiDatabaseMigrationsAssembly>();
        return;
    }

    if (provider == "SqlServer")
    {
        b.UseSqlServer("Data Source=localhost;Initial Catalog=TenantDb;Integrated Security=True;")
            .UseMigrationNamespace(new SqlServerMigrationNamespace())
            .ReplaceService<IMigrationsAssembly, MultiDatabaseMigrationsAssembly>();
        return;
    }
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