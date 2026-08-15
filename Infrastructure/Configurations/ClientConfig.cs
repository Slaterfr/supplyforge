using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Location)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.ContactInfo)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.DateCreated)
                .IsRequired();

            builder.Property(c => c.DateUpdated)
                .IsRequired();

            // Relationship: User -> Client (One-to-Many)
            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Client -> Order (One-to-Many)
            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
