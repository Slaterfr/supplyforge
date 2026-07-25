using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class ShipmentConfig : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");

            builder.Property(s => s.ShipmentDate)
                .IsRequired();

            builder.Property(s => s.DeliveryDate)
                .IsRequired();




        }
    }
}
