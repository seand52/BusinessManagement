using Microsoft.AspNetCore.Mvc;
using BusinessManagement.Commands;
using BusinessManagement.Commands.SalesOrders;
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
        private readonly IAwsPublisher _awsPublisher;

        public SalesOrdersController(IMediator mediator, IAwsPublisher _awsPublisher)
        {
            _mediator = mediator;
            this._awsPublisher = _awsPublisher;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDetailDto>> Get(int id)
        {
            var result = await _mediator.Send(new GetSalesOrderQuery(id, GetUserId()));
            return result != null ? Ok(result) : NotFound();
        }
        
        [HttpGet("{id}/generate")]
        public async Task<ActionResult<SalesOrderDetailDto>> Generate(int id)
        {
            var key = $"salesOrders/{id}";
            var pdf = await _awsPublisher.Download(key);
            if (pdf != null)
            {
                return File(pdf, "application/pdf", $"{id}.pdf");
            }
            var result = await _mediator.Send(new GetSalesOrderQuery(id, GetUserId()));
            var pdfBytes = await _mediator.Send(new SalesOrderCreatedEvent(result));
            return File(pdfBytes, "application/pdf", $"{result.Id}.pdf");
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedList<SalesOrderDto>>> GetSalesOrders([FromQuery] PaginationFilter filter, [FromQuery] SearchParams? searchParams = null)
        {
            var result = await _mediator.Send(new GetAllSalesOrdersQuery(filter, searchParams, GetUserId()));
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
            var pdfBytes = await _mediator.Send(new SalesOrderCreatedEvent(result));
            return File(pdfBytes, "application/pdf", $"{result.Id}.pdf");
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
            await _mediator.Send(new SalesOrderUpdatedEvent(id, GetUserId()));
            return success ? NoContent() : BadRequest();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteSalesOrderRequest(id, GetUserId()));
            return result ? NoContent() : BadRequest();
        }
        
        [Route("{salesOrderId}/convertToInvoice")]
        [HttpGet]
        public async Task<ActionResult<SalesOrderDetailDto>> ConvertToInvoice(int salesOrderId)
        {
            var result = await _mediator.Send(new ConvertSalesOrderToInvoiceRequest(salesOrderId, GetUserId()));
            if (result == null)
            {
                return BadRequest("Sales order not able to be converted to invoice");
            }
            var pdfBytes = await _mediator.Send(new InvoiceCreatedEvent(result));
            return File(pdfBytes, "application/pdf", $"{result.Id}.pdf");
        }
    }
}