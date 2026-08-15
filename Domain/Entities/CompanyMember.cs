namespace SupplyForge.Domain.Entities
{
    public sealed class CompanyMember
    {
        public Guid UserId { get; private set; }
        public Guid CompanyId { get; private set; }
        public int RoleId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User User { get; private set; } // Navigation property
        public Company Company { get; private set; } // Navigation property
        public Role Role { get; private set; } // Navigation property

        private CompanyMember() { }

        public CompanyMember(Guid userId, Guid companyId, int roleId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }

            if (companyId == Guid.Empty)
            {
                throw new ArgumentException("Company ID cannot be empty.", nameof(companyId));
            }

            if (roleId <= 0)
            {
                throw new ArgumentException("Role ID must be greater than zero.", nameof(roleId));
            }

            UserId = userId;
            CompanyId = companyId;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
