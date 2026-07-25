namespace SupplyForge.Domain.Entities
{
    public sealed class Shipment
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public DateTime ShipmentDate { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public enum Status
        {
            Pending,
            InTransit,
            Delivered,
            Cancelled
        }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }

        private Shipment()
        {
            // Required by EF Core
        }

        public Shipment(Guid vehicleId, DateTime shipmentDate, DateTime deliveryDate, Guid productId, int quantity)
        {
            if (vehicleId == Guid.Empty)
            {
                throw new ArgumentException("Vehicle ID cannot be empty.", nameof(vehicleId));
            }
            if (shipmentDate > deliveryDate)
            {
                throw new ArgumentException("Shipment date cannot be later than delivery date.");
            }
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
            }
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            ShipmentDate = shipmentDate;
            DeliveryDate = deliveryDate;
            ProductId = productId;
            Quantity = quantity;
            DateCreated = DateTime.UtcNow;

        }
    }
}
