using SupplyForge.Domain.Enums;

namespace SupplyForge.Domain.Entities
{
    public sealed class Order
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        public Client Client { get; private set; } // Navigation property
        public ICollection<Shipment> Shipments { get; private set; } = new List<Shipment>();

        private Order() { }

        public Order(Guid clientId)
        {
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
            }

            Id = Guid.NewGuid();
            ClientId = clientId;
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.UtcNow;
            Status = OrderStatus.Pending;
        }
    }
}
