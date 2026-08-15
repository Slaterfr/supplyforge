namespace SupplyForge.Domain.Entities
{
    public sealed class Role
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ICollection<CompanyMember> Members { get; private set; } = new List<CompanyMember>();

        private Role() { }

        public Role(int id, string name, string description)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Role ID must be greater than zero.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Role description cannot be null or empty.", nameof(description));
            }

            Id = id;
            Name = name;
            Description = description;
        }
    }
}
