using SupplyForge.Domain.Entities;

namespace SupplyForge.App.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();

        Task<Product> GetProductByIdAsync(Guid Id);
        Task AddProductAsync(ProductDTO product);
        Task UpdateProductAsync(Guid id, ProductDTO data);
        Task DeleteProductAsync(Guid id);
    }
}
