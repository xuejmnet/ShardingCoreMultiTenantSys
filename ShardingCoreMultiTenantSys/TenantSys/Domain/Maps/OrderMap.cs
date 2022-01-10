using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShardingCoreMultiTenantSys.TenantSys.Domain.Entities;

namespace ShardingCoreMultiTenantSys.TenantSys.Domain.Maps
{
    public class OrderMap:IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
            builder.HasQueryFilter(o => o.IsDeleted == false);
            builder.ToTable(nameof(Order));
        }
    }
}
