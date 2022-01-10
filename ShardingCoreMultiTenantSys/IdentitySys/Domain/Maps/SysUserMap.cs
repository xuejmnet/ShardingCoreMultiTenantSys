using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShardingCoreMultiTenantSys.IdentitySys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.IdentitySys.Domain.Maps
{
    public class SysUserMap:IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Password).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.HasQueryFilter(o => o.IsDeleted == false);
            builder.ToTable(nameof(SysUser));
        }
    }
}
