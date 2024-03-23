using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllInvoicesHandler: IRequestHandler<GetAllInvoicesQuery, PagedList<InvoiceDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetAllInvoicesHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task<PagedList<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var invoices = await _invoiceRepository.GetInvoices(validFilter, request.SearchTerm, request.UserId);
        var invoiceDtos = invoices.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<InvoiceDto>(invoiceDtos, invoices.TotalCount, invoices.Page, invoices.PageSize);
    }
}