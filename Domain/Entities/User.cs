namespace SupplyForge.Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<CompanyMember> Memberships { get; private set; } = new List<CompanyMember>();

        private User() { }

        public User(Guid id, string name, string email, string passwordHash)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("User name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("User email cannot be null or empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash cannot be null or empty.", nameof(passwordHash));
            }

            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
