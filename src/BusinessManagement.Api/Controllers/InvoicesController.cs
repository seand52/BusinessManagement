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
    public class InvoicesController : BusinessManagementController
    {
        private readonly IMediator _mediator;
        private readonly IAwsPublisher _awsPublisher;

        public InvoicesController(IMediator mediator, IAwsPublisher awsPublisher)
        {
            _mediator = mediator;
            _awsPublisher = awsPublisher;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDetailDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetInvoiceQuery(id, GetUserId()));
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpGet("{id}/generate")]
        public async Task<ActionResult<InvoiceDetailDto>> GenerateInvoice(int id)
        {
            var key = $"invoices/{id}";
            var pdf = await _awsPublisher.Download(key);
            if (pdf != null)
            {
                return File(pdf, "application/pdf", $"{id}.pdf");
            }
            var result = await _mediator.Send(new GetInvoiceQuery(id, GetUserId()));
            var pdfBytes = await _mediator.Send(new InvoiceCreatedEvent(result));
            return File(pdfBytes, "application/pdf", $"{result.Id}.pdf");
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedList<InvoiceDto>>> GetInvoices([FromQuery] PaginationFilter filter, [FromQuery] SearchParams? searchParams)
        {
            var result = await _mediator.Send(new GetAllInvoicesQuery(filter, searchParams, GetUserId()));
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
            var pdfBytes = await _mediator.Send(new InvoiceCreatedEvent(result));
            return File(pdfBytes, "application/pdf", $"{result.Id}.pdf");
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
            await _mediator.Send(new InvoiceUpdatedEvent(id, GetUserId()));
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