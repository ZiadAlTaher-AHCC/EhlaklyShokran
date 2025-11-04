using EhlaklyShokran.Domain.BarberTasks.Cosmetics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EhlaklyShokran.Infrastructure.Data.Configurations;

public class CosmeticConfiguration : IEntityTypeConfiguration<Cosmetic>
{
    public void Configure(EntityTypeBuilder<Cosmetic> builder)
    {
        builder.HasKey(p => p.Id).IsClustered(false);
        builder.Property(rt => rt.Id).ValueGeneratedNever();

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Cost)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Quantity)
               .IsRequired();
    }
}