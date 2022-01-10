using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using ShardingCore.Core.VirtualDatabase.VirtualDataSources.Abstractions;
using ShardingCore.Sharding.ReadWriteConfigurations;
using ShardingCore.TableExists;
using ShardingCore.TableExists.Abstractions;
using ShardingCoreMultiTenantSys.DbContexts;

namespace ShardingCoreMultiTenantSys.IdentitySys.ShardingConfigs
{
    public class SqlShardingConfiguration : AbstractVirtualDataSourceConfigurationParams<TenantDbContext>
    {
        private static readonly ILoggerFactory efLogger = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
        });
        public override string ConfigId { get; }
        public override int Priority { get; }
        public override string DefaultDataSourceName { get; }
        public override string DefaultConnectionString { get; }
        public override ITableEnsureManager TableEnsureManager { get; }

        private readonly DbTypeEnum _dbType;
        public SqlShardingConfiguration(ShardingTenantOptions options)
        {
            ConfigId = options.ConfigId;
            Priority = options.Priority;
            DefaultDataSourceName = options.DefaultDataSourceName;
            DefaultConnectionString = options.DefaultConnectionString;
            _dbType = options.DbType;
            //用来快速判断是否存在数据库中的表
            if (_dbType == DbTypeEnum.MSSQL)
            {
                TableEnsureManager = new SqlServerTableEnsureManager<TenantDbContext>();
            }
            else if (_dbType == DbTypeEnum.MYSQL)
            {
                TableEnsureManager = new MySqlTableEnsureManager<TenantDbContext>();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public override DbContextOptionsBuilder UseDbContextOptionsBuilder(string connectionString,
            DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            switch (_dbType)
            {
                case DbTypeEnum.MSSQL:
                    {
                        dbContextOptionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(efLogger);
                    }
                    break;
                case DbTypeEnum.MYSQL:
                    {
                        dbContextOptionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version())).UseLoggerFactory(efLogger);
                    }
                    break;
                default: throw new NotImplementedException();
            }
            return dbContextOptionsBuilder;
        }

        public override DbContextOptionsBuilder UseDbContextOptionsBuilder(DbConnection dbConnection,
            DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            switch (_dbType)
            {
                case DbTypeEnum.MSSQL:
                {
                    dbContextOptionsBuilder.UseSqlServer(dbConnection).UseLoggerFactory(efLogger);
                    }
                    break;
                case DbTypeEnum.MYSQL:
                {
                    dbContextOptionsBuilder.UseMySql(dbConnection, new MySqlServerVersion(new Version())).UseLoggerFactory(efLogger);
                    }
                    break;
                default: throw new NotImplementedException();
            }
            return dbContextOptionsBuilder;
        }
    }
}
