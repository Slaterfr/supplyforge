using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class VehicleConfig : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.PlateNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(v => v.PlateNumber)
                .IsUnique();

            builder.Property(v => v.MaxLoad)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(v => v.CreatedAt)
                .IsRequired();

            // Relationship: Vehicle -> Shipment (One-to-Many)
            builder.HasMany(v => v.Shipments)
                .WithOne(s => s.Vehicle)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
