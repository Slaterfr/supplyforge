using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using SupplyForge.Database;
using SupplyForge.Domain.Entities;

namespace SupplyForge.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

    }
}