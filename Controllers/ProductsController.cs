using Microsoft.AspNetCore.Mvc;
using BusinessManagementApi.Models;
using BusinessManagement.Commands;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : BusinessManagementController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetProductQuery(id, GetUserId()));
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpGet]
        public  async Task<ActionResult<PagedList<ProductDto>>> GetProducts([FromQuery] PaginationFilter filter, [FromQuery] string? SearchTerm)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(filter, SearchTerm, GetUserId()));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _mediator.Send(new CreateProductRequest(product, GetUserId()));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id, [FromBody] UpdateProductDto? product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var success = await _mediator.Send(new UpdateProductRequest(product, id, GetUserId()));
            
            return success ? NoContent() : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductRequest(id, GetUserId()));
            return result ? NoContent() : BadRequest();
        }
    }
}