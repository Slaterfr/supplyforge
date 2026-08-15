using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(r => r.Name)
                .IsUnique();

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(250);

            // Relationship: Role -> CompanyMember (One-to-Many)
            builder.HasMany(r => r.Members)
                .WithOne(cm => cm.Role)
                .HasForeignKey(cm => cm.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
