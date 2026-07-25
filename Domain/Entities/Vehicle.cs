namespace SupplyForge.Domain.Entities
{
    public class Vehicle
    {
        Guid Id { get; set; }
        public string PlateNumber { get; set; }
        public decimal MaxLoad { get; set; }
        public enum Status
        {
            Available,
            InUse,
            Maintenance
        }
    }
}
