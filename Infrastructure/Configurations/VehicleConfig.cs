using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class VehicleConfig : IEntityTypeConfiguration<Vehicle>
    {

        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");
            builder.Property(v => v.PlateNumber)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(v => v.MaxLoad)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
