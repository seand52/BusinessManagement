using Microsoft.AspNetCore.Mvc;
using BusinessManagement.Commands;
using BusinessManagement.Filter;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace BusinessManagement.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class InvoicesController : BusinessManagementController
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDetailDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetInvoiceQuery(id, GetUserId()));
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpGet]
        public async Task<ActionResult<Invoice>> GetInvoices([FromQuery] PaginationFilter filter, [FromQuery] string? SearchTerm)
        {
            var result = await _mediator.Send(new GetAllInvoicesQuery(filter, SearchTerm, GetUserId()));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDetailDto>> Create([FromBody] CreateInvoiceDto invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _mediator.Send(new CreateInvoiceRequest(invoice, GetUserId()));
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateInvoiceDto? invoice)
        {
            if (invoice == null)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var success = await _mediator.Send(new UpdateInvoiceRequest(invoice, id, GetUserId()));
            
            return success ? NoContent() : BadRequest();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteInvoiceRequest(id, GetUserId()));
            return result ? NoContent() : BadRequest();
        }
        
    }
}