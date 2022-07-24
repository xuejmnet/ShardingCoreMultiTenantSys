namespace ShardingCoreMultiTenantSys.MigrationsAssemblies;

public class MySqlMigrationNamespace:IMigrationNamespace
{
    public string GetNamespace()
    {
        return "ShardingCoreMultiTenantSys.Migrations.MySql";
    }
}