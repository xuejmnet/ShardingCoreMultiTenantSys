namespace ShardingCoreMultiTenantSys.MigrationsAssemblies;

public class SqlServerMigrationNamespace:IMigrationNamespace
{
    public string GetNamespace()
    {
        return "ShardingCoreMultiTenantSys.Migrations.SqlServer";
    }
}