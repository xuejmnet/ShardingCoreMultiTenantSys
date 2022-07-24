using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ShardingCoreMultiTenantSys.MigrationsAssemblies;

public class MigrationNamespaceExtension : IDbContextOptionsExtension
{
    public IMigrationNamespace MigrationNamespace { get; }

    public MigrationNamespaceExtension(IMigrationNamespace migrationNamespace)
    {
        MigrationNamespace = migrationNamespace;
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton<IMigrationNamespace>(sp => MigrationNamespace);
    }

    public void Validate(IDbContextOptions options)
    {
    }


    public DbContextOptionsExtensionInfo Info => new MigrationNamespaceExtensionInfo(this);

    private class MigrationNamespaceExtensionInfo : DbContextOptionsExtensionInfo
    {
        private readonly MigrationNamespaceExtension _migrationNamespaceExtension;

        public MigrationNamespaceExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
            _migrationNamespaceExtension = (MigrationNamespaceExtension)extension;
        }

        public override int GetServiceProviderHashCode() =>
            _migrationNamespaceExtension.MigrationNamespace.GetNamespace().GetHashCode();

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool IsDatabaseProvider => false;
        public override string LogFragment => "MigrationNamespaceExtension";
    }
}