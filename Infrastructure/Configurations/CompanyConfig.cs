using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            // Relationship: Company -> CompanyMembers (One-to-Many)
            builder.HasMany(c => c.Members)
                .WithOne(cm => cm.Company)
                .HasForeignKey(cm => cm.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
