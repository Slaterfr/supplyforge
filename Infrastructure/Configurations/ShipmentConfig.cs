using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class ShipmentConfig : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.OrderId)
                .IsRequired();

            builder.Property(s => s.VehicleId)
                .IsRequired();

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder.Property(s => s.Quantity)
                .IsRequired();

            builder.Property(s => s.ShipmentDate)
                .IsRequired();

            builder.Property(s => s.DeliveryDate)
                .IsRequired();

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(s => s.DateCreated)
                .IsRequired();

            // Relationship: Order -> Shipment (One-to-Many)
            builder.HasOne(s => s.Order)
                .WithMany(o => o.Shipments)
                .HasForeignKey(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Vehicle -> Shipment (One-to-Many)
            builder.HasOne(s => s.Vehicle)
                .WithMany(v => v.Shipments)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);


            // Relationship: Shipment -> ShipmentItem (One-to-Many)
            builder.HasMany(s => s.ShipmentItems)
                .WithOne(si => si.Shipment)
                .HasForeignKey(si => si.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
