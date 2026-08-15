using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class ShipmentItemConfig : IEntityTypeConfiguration<ShipmentItem>
    {
        public void Configure(EntityTypeBuilder<ShipmentItem> builder)
        {
            builder.ToTable("ShipmentItems");

            builder.HasKey(si => si.Id);

            builder.Property(si => si.ShipmentId)
                .IsRequired();

            builder.Property(si => si.ProductId)
                .IsRequired();

            builder.Property(si => si.UnitQuantity)
                .IsRequired();

            builder.Property(si => si.UnitWeight)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Relationship: Shipment -> ShipmentItem (One-to-Many)
            builder.HasOne(si => si.Shipment)
                .WithMany(s => s.ShipmentItems)
                .HasForeignKey(si => si.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
