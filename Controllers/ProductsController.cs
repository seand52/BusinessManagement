using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using AutoMapper;
using BusinessManagement.Filter;
using BusinessManagementApi.Services;
using BusinessManagementApi.Dto;
using Microsoft.AspNetCore.Authorization;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BusinessManagementController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            Product? product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDto>(product));
        }
        
        [HttpGet]
        public  async Task<ActionResult<Product>> GetProducts([FromQuery] PaginationFilter filter, [FromQuery] string? SearchTerm)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var products = await _productService.GetProducts(validFilter, SearchTerm);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            var isSuccess = await _productService.CreateProduct(productEntity, ModelState);

            if (!isSuccess)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(Get), new { id = productEntity.Id }, _mapper.Map<ProductDto>(productEntity));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, [FromBody] UpdateProductDto? product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var productToUpdate = await _productService.GetProductById(id);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            var productEntity = _mapper.Map<Product>(product);
            await _productService.UpdateProduct(productToUpdate, productEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Product? product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(product);
            return Ok();
        }
    }
}