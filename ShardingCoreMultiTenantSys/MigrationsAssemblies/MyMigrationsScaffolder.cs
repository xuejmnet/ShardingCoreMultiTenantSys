using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Design;

namespace ShardingCoreMultiTenantSys.MigrationsAssemblies;

public class MyMigrationsScaffolder: MigrationsScaffolder
{
    private readonly Type _contextType;
    public MyMigrationsScaffolder(MigrationsScaffolderDependencies dependencies) : base(dependencies)
    {
        _contextType = dependencies.CurrentContext.Context.GetType();
    }
    protected override string GetDirectory(string projectDir, string? siblingFileName, string subnamespace)
    {
        var defaultDirectory = Path.Combine(projectDir, Path.Combine(subnamespace.Split('.')));

        if (siblingFileName != null)
        {
            if (!siblingFileName.StartsWith(_contextType.Name + "ModelSnapshot."))
            {
                var siblingPath = TryGetProjectFile(projectDir, siblingFileName);
                if (siblingPath != null)
                {
                    var lastDirectory = Path.GetDirectoryName(siblingPath)!;
                    if (!defaultDirectory.Equals(lastDirectory, StringComparison.OrdinalIgnoreCase))
                    {
                        Dependencies.OperationReporter.WriteVerbose(DesignStrings.ReusingNamespace(siblingFileName));

                        return lastDirectory;
                    }
                }
            }
        }

        return defaultDirectory;
    }
}