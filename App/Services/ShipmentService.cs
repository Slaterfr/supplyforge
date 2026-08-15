using SupplyForge.Infrastructure;
using SupplyForge.Domain.Entities;
using SupplyForge.Database;
using Microsoft.EntityFrameworkCore;
using SupplyForge.App.Interfaces;

namespace SupplyForge.App.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly ApplicationDbContext _context;
        public ShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Shipment>> GetShipments(Guid? OrderId = null)
        {
            var query = _context.Shipments.AsQueryable();
            if (OrderId != null)
            {
                query = query.Take(100).Where(s => s.Id == OrderId).OrderByDescending(s => s.DateCreated);
            }
            else
            {
                query = query.Take(100).OrderByDescending(s => s.DateCreated);
            }
            return await query.ToListAsync();
        }

        public async Task AddShipmentAsync(ShipmentDTO shipment)
        {
            // Nota: Esta es una implementación básica
            // Necesitarás ajustar según tu lógica de negocio
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShipmentAsync(Guid id, ShipmentDTO data)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteShipmentAsync(Guid id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return;

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
        }
    }
}
