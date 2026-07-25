using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplyForge.Domain.Enums;


namespace SupplyForge.Domain.Entities
{
    public sealed class Vehicle
    {
        public Guid Id { get; private set; }
        public string PlateNumber { get; private set; }
        public decimal MaxLoad { get; private set; }
        public VehicleStatus Status { get; private set; } = VehicleStatus.Available;
        public ICollection<Shipment> Shipments { get; private set; } = new List<Shipment>();

        private Vehicle()
        {
            // For EF Core
        }

        public Vehicle(string plateNumber, decimal maxLoad)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                throw new ArgumentException("Plate number cannot be empty.", nameof(plateNumber));
            }
            if (maxLoad <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLoad), "Max load must be greater than zero.");
            }
            Id = Guid.NewGuid();
            PlateNumber = plateNumber;
            MaxLoad = maxLoad;
        }

    }

}
