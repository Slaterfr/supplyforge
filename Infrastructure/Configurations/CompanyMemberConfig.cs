using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Infrastructure.Configurations
{
    public sealed class CompanyMemberConfig : IEntityTypeConfiguration<CompanyMember>
    {
        public void Configure(EntityTypeBuilder<CompanyMember> builder)
        {
            builder.ToTable("CompanyMembers");

            // Composite primary key
            builder.HasKey(cm => new { cm.UserId, cm.CompanyId });

            builder.Property(cm => cm.RoleId)
                .IsRequired();

            builder.Property(cm => cm.CreatedAt)
                .IsRequired();

            // Relationship: User -> CompanyMember (One-to-Many)
            builder.HasOne(cm => cm.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Company -> CompanyMember (One-to-Many)
            builder.HasOne(cm => cm.Company)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Role -> CompanyMember (One-to-Many)
            builder.HasOne(cm => cm.Role)
                .WithMany(r => r.Members)
                .HasForeignKey(cm => cm.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
