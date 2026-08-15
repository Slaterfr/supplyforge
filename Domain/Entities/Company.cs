namespace SupplyForge.Domain.Entities
{
    public sealed class Company
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public ICollection<CompanyMember> Members { get; private set; } = new List<CompanyMember>();

        private Company() { }

        public Company(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Company name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Company address cannot be null or empty.", nameof(address));
            }

            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
