using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesContract;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> productRepo;

        public ProductsController(IGenericRepository<Product> productRepo)
        {
            this.productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productRepo.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await productRepo.GetAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Not Found", StatusCode = 404 });
            }
            return Ok(product);
        }
    }
}
