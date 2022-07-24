using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShardingCoreMultiTenantSys.MigrationsAssemblies;

namespace ShardingCoreMultiTenantSys.Extensions;

public static class MigrationExtension
{
    
    public static DbContextOptionsBuilder UseMigrationNamespace(this DbContextOptionsBuilder optionsBuilder,IMigrationNamespace migrationNamespace)
    {
        var shardingWrapExtension = optionsBuilder.CreateOrGetExtension(migrationNamespace);
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(shardingWrapExtension);
        return optionsBuilder;
    }

    private static MigrationNamespaceExtension CreateOrGetExtension(
        this DbContextOptionsBuilder optionsBuilder, IMigrationNamespace migrationNamespace)
        => optionsBuilder.Options.FindExtension<MigrationNamespaceExtension>() ??
           new MigrationNamespaceExtension(migrationNamespace);
}