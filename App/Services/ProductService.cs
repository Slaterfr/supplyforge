using Microsoft.EntityFrameworkCore;
using SupplyForge.Infrastructure;
using SupplyForge.Domain.Entities;
using SupplyForge.Database;
using SupplyForge.App;
using SupplyForge.App.Interfaces;


namespace SupplyForge.App.Services
{
    public class ProductService : IProductService

    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Take(100).ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task AddProductAsync(ProductDTO data)
        {
            var product = new Product
            (
                data.Name,
                data.Description,
                data.Price,
                data.Weight
            );

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Guid id, ProductDTO data)
        {

            await _context.Products.Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, data.Name)
                    .SetProperty(p => p.Description, data.Description)
                    .SetProperty(p => p.Price, data.Price)
                    .SetProperty(p => p.Weight, data.Weight));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
