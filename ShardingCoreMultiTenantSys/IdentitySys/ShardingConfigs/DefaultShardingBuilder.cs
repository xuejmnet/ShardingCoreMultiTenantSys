using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ShardingCore;
using ShardingCore.Core.RuntimeContexts;
using ShardingCore.TableExists;
using ShardingCore.TableExists.Abstractions;
using ShardingCoreMultiTenantSys.DbContexts;
using ShardingCoreMultiTenantSys.TenantSys.Shardings;

namespace ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs;

public class DefaultShardingBuilder:IShardingBuilder
{
    public static readonly ILoggerFactory efLogger = LoggerFactory.Create(builder =>
    {
        builder.AddFilter((category, level) =>
            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
    });
    private readonly IServiceProvider _serviceProvider;

    public DefaultShardingBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IShardingRuntimeContext Build(ShardingTenantOptions tenantOptions)
    {
        var shardingRuntimeBuilder = new ShardingRuntimeBuilder<TenantDbContext>()
            .UseRouteConfig(o =>
            {
                if (tenantOptions.OrderShardingType == OrderShardingTypeEnum.Mod)
                {
                    o.AddShardingTableRoute<OrderModTableRoute>();
                }
                if (tenantOptions.OrderShardingType == OrderShardingTypeEnum.ByMonth)
                {
                    o.AddShardingTableRoute<OrderMonthTableRoute>();
                }
            }).UseConfig(o =>
            {
                o.ThrowIfQueryRouteNotMatch = false;
                o.UseShardingQuery((conStr, builder) =>
                {
                    if (tenantOptions.DbType == DbTypeEnum.MYSQL)
                    {
                        builder.UseMySql(conStr, new MySqlServerVersion(new Version())); 
                    }
                    if (tenantOptions.DbType == DbTypeEnum.MSSQL)
                    {
                        builder.UseSqlServer(conStr); 
                    }
                    builder.UseLoggerFactory(efLogger)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                o.UseShardingTransaction((connection, builder) =>
                {
                    if (tenantOptions.DbType == DbTypeEnum.MYSQL)
                    {
                        builder
                            .UseMySql(connection, new MySqlServerVersion(new Version()));
                    }
                    if (tenantOptions.DbType == DbTypeEnum.MSSQL)
                    {
                        builder.UseSqlServer(connection); 
                    }
                    builder.UseLoggerFactory(efLogger)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                o.AddDefaultDataSource(tenantOptions.DefaultDataSourceName,tenantOptions.DefaultConnectionString);
                //注意这个迁移必须要十分重要
                //注意这个迁移必须要十分重要
                //注意这个迁移必须要十分重要
                //注意这个迁移必须要十分重要
                o.UseShardingMigrationConfigure(b =>
                {
                    if (tenantOptions.DbType == DbTypeEnum.MYSQL)
                    {
                        b.ReplaceService<IMigrationsSqlGenerator, ShardingMySqlMigrationsSqlGenerator>();
                    }
                    if (tenantOptions.DbType == DbTypeEnum.MSSQL)
                    {
                        b.ReplaceService<IMigrationsSqlGenerator, ShardingMySqlMigrationsSqlGenerator>();
                    }
                });
            });
        
        if (tenantOptions.DbType == DbTypeEnum.MYSQL)
        {
            shardingRuntimeBuilder.ReplaceService<ITableEnsureManager, MySqlTableEnsureManager>(ServiceLifetime
                .Singleton);
        }
        if (tenantOptions.DbType == DbTypeEnum.MSSQL)
        {
            shardingRuntimeBuilder.ReplaceService<ITableEnsureManager, SqlServerTableEnsureManager>(ServiceLifetime
                .Singleton);
        }
        return shardingRuntimeBuilder.Build(_serviceProvider);
    }
}