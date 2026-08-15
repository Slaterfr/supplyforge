using SupplyForge.App;
using SupplyForge.Domain.Entities;

namespace SupplyForge.App.Interfaces
{
    public interface IShipmentService
    {
        Task <List<Shipment>> GetShipments(Guid? OrderId);
        Task AddShipmentAsync(ShipmentDTO shipment);
        Task UpdateShipmentAsync(Guid id, ShipmentDTO data);
        Task DeleteShipmentAsync(Guid id);
    }
}
