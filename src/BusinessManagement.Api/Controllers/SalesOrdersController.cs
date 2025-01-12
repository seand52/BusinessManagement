using Microsoft.AspNetCore.Mvc;
using BusinessManagement.Commands;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Extensions.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SalesOrdersController : BusinessManagementController
    {
        private readonly IMediator _mediator;

        public SalesOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDetailDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetSalesOrderQuery(id, GetUserId()));
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedList<SalesOrderDto>>> GetSalesOrders([FromQuery] PaginationFilter filter, [FromQuery] string? SearchTerm)
        {
            var result = await _mediator.Send(new GetAllSalesOrdersQuery(filter, SearchTerm, GetUserId()));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SalesOrderDetailDto>> Create([FromBody] CreateSalesOrderDto salesOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _mediator.Send(new CreateSalesOrderRequest(salesOrder, GetUserId()));
            await _mediator.Publish(new SalesOrderCreatedEvent(result));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateSalesOrderDto? salesOrder)
        {
            if (salesOrder == null)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var success = await _mediator.Send(new UpdateSalesOrderRequest(salesOrder, id, GetUserId()));
            
            return success ? NoContent() : BadRequest();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSalesOrderRequest(id, GetUserId()));
            return result ? NoContent() : BadRequest();
        }
        
        [Route("{salesOrderId}/convertToInvoice")]
        [HttpPost]
        public async Task<ActionResult<SalesOrderDetailDto>> ConvertToInvoice(int salesOrderId)
        {
            var result = await _mediator.Send(new ConvertSalesOrderToInvoiceRequest(salesOrderId, GetUserId()));
            // TODO: handle different error types
            return result ? Ok() : BadRequest();
        }
    }
}