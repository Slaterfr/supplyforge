using SupplyForge.Infrastructure;
using SupplyForge.Domain.Entities;
using SupplyForge.Database;
using Microsoft.EntityFrameworkCore;

namespace SupplyForge.App.Services
{
    public class ShipmentService
    {
        private readonly ApplicationDbContext _context;
        public ShipmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Shipment>> GetShipments()
        {
            return await _context.Shipments.Take(100).ToListAsync();

        }

    }
}
