using EhlaklyShokran.Domain.BarberTasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EhlaklyShokran.Infrastructure.Data.Configurations;

public class BarberTaskConfiguration :  IEntityTypeConfiguration<BarberTask>
{
    public void Configure(EntityTypeBuilder<BarberTask> builder)
    {
        builder.HasKey(rt => rt.Id).IsClustered(false);

        builder.Property(rt => rt.Id).ValueGeneratedNever();

        builder.Property(rt => rt.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(w => w.EstimatedDurationInMins).HasConversion<string>().IsRequired();

        builder.Property(rt => rt.EstimatedDurationInMins)
       .IsRequired();

        builder.Property(rt => rt.LaborCost)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.HasMany(c => c.Cosmetics)
           .WithOne()
           .HasForeignKey("BarberTaskId")
           .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.Cosmetics)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}