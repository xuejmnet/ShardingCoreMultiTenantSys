using Microsoft.EntityFrameworkCore;
using ShardingCoreMultiTenantSys.IdentitySys.Domain.Maps;

namespace ShardingCoreMultiTenantSys.IdentitySys
{
    public class IdentityDbContext:DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SysUserMap());
            modelBuilder.ApplyConfiguration(new SysUserTenantConfigMap());
        }
    }
}
