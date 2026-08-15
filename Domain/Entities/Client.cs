namespace SupplyForge.Domain.Entities
{
    public sealed class Client
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Location { get; private set; }
        public string ContactInfo { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; private set; }

        public User User { get; private set; } // Navigation property
        public ICollection<Order> Orders { get; private set; } = new List<Order>();

        private Client()
        {
            // for EF Core
        }

        public Client(Guid userId, string name, string location, string contactInfo)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Client name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException("Client location cannot be null or empty.", nameof(location));
            }

            if (contactInfo == null)
            {
                throw new ArgumentNullException(nameof(contactInfo), "Contact info cannot be null.");
            }

            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            Location = location;
            ContactInfo = contactInfo;
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
        }
    }
}
