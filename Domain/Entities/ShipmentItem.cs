namespace SupplyForge.Domain.Entities
{
    public sealed class ShipmentItem
    {
        public Guid ShipmentId { get; set; }
        public Guid ProductId { get; set; }
        public int UnitQuantity { get; set; }
        public decimal UnitWeight { get; set; }
        public decimal TotalWeight => UnitWeight * UnitQuantity;

        
    }
}
