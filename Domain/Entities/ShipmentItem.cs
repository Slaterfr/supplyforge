namespace SupplyForge.Domain.Entities
{
    public sealed class ShipmentItem
    {
        public Guid Id { get; private set; }
        public Guid ShipmentId { get; private set; }
        public Guid ProductId { get; private set; }
        public int UnitQuantity { get; private set; }
        public decimal UnitWeight { get; private set; }
        public decimal UnitPrice { get; private set; }

        // Calculated properties
        public decimal TotalWeight => UnitWeight * UnitQuantity;
        public decimal TotalPrice => UnitPrice * UnitQuantity;

        // Navigation properties
        public Shipment Shipment { get; private set; }
        public Product Product { get; private set; }

        private ShipmentItem() { } // For EF Core

        public ShipmentItem(Guid shipmentId, Guid productId, int unitQuantity, decimal unitWeight, decimal unitPrice)
        {
            if (shipmentId == Guid.Empty)
            {
                throw new ArgumentException("Shipment ID cannot be empty.", nameof(shipmentId));
            }

            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
            }

            if (unitQuantity <= 0)
            {
                throw new ArgumentException("Unit quantity must be a positive integer.", nameof(unitQuantity));
            }

            if (unitWeight < 0)
            {
                throw new ArgumentException("Unit weight cannot be negative.", nameof(unitWeight));
            }

            if (unitPrice < 0)
            {
                throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));
            }

            Id = Guid.NewGuid();
            ShipmentId = shipmentId;
            ProductId = productId;
            UnitQuantity = unitQuantity;
            UnitWeight = unitWeight;
            UnitPrice = unitPrice;
        }
    }
}
