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

            builder.Property(si => si.UnitQuantity)
                .IsRequired()
                .HasColumnType("int");
            builder.Property(si => si.Id)
                .IsRequired();
            

            builder.Property(si => si.UnitWeight) .IsRequired().HasMaxLength(64).HasColumnType("decimal(18,2)");
            builder.Property(si => si.UnitPrice) .IsRequired().HasMaxLength(64).HasColumnType("decimal(18,2)");

            builder.HasOne(si => si.Shipment)
                .WithMany(s => s.ShipmentItems)
                .HasForeignKey(si => si.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
