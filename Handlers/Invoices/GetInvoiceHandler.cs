using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetInvoiceHandler: IRequestHandler<GetInvoiceQuery, InvoiceDetailDto?>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetInvoiceHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    
    public async Task<InvoiceDetailDto?> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await _invoiceRepository.GetInvoiceById(request.InvoiceId, request.UserId);
        return invoice.ToDto();
    }
}