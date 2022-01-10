using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.IdentitySys.Domain.Maps
{
    public class SysUserTenantConfigMap:IEntityTypeConfiguration<SysUserTenantConfig>
    {
        public void Configure(EntityTypeBuilder<SysUserTenantConfig> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(o => o.UserId).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(o => o.ConfigJson).IsRequired().HasMaxLength(2000);
            builder.HasQueryFilter(o => o.IsDeleted == false);
            builder.ToTable(nameof(SysUserTenantConfig));
        }
    }
}
