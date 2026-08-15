using SupplyForge.Domain.Enums;

namespace SupplyForge.Domain.Entities
{
    public sealed class Shipment
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid ProductId { get; private set; }
        public DateTime ShipmentDate { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public int Quantity { get; private set; }
        public ShipmentStatus Status { get; private set; } = ShipmentStatus.Pending;
        public DateTime DateCreated { get; private set; }

        public Order Order { get; private set; } // Navigation property
        public Vehicle Vehicle { get; private set; } // Navigation property
        public Product Product { get; private set; } // Navigation property
        public ICollection<ShipmentItem> ShipmentItems { get; private set; } = new List<ShipmentItem>();

        private Shipment()
        {
            // Required by EF Core
        }

        public Shipment(Guid orderId, Guid vehicleId, Guid productId, DateTime shipmentDate, DateTime deliveryDate, int quantity)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
            }

            if (vehicleId == Guid.Empty)
            {
                throw new ArgumentException("Vehicle ID cannot be empty.", nameof(vehicleId));
            }

            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
            }

            if (shipmentDate > deliveryDate)
            {
                throw new ArgumentException("Shipment date cannot be later than delivery date.");
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }

            Id = Guid.NewGuid();
            OrderId = orderId;
            VehicleId = vehicleId;
            ProductId = productId;
            ShipmentDate = shipmentDate;
            DeliveryDate = deliveryDate;
            Quantity = quantity;
            Status = ShipmentStatus.Pending;
            DateCreated = DateTime.UtcNow;
        }
    }
}
