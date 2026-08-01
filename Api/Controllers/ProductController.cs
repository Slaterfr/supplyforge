using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SupplyForge.App.Interfaces;

namespace SupplyForge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProducts), new { id = product.Name }, product);
        }
    }
}
