using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesContract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<ProductBrand> brandRepo;
        private readonly IGenericRepository<ProductCategory> categoryRepo;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> brandRepo,
            IGenericRepository<ProductCategory> categoryRepo,
            IMapper mapper)
        {
            this.productRepo = productRepo;
            this.brandRepo = brandRepo;
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var spec = new ProductWithBrandAndCategorySpec();
            var products = await productRepo.GetAllWithSpecAsync(spec);
            return Ok(mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products));
        }

        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpec();
            var product = await productRepo.GetWithSpecAsync(spec);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(mapper.Map<Product, ProductDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var brands = await brandRepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories()
        {
            var categories = await categoryRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
