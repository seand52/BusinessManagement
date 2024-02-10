using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllInvoicesHandler: IRequestHandler<GetAllInvoicesQuery, PagedList<Invoice>>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetAllInvoicesHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task<PagedList<Invoice>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var invoices = await _invoiceRepository.GetInvoices(validFilter, request.SearchTerm, request.UserId);
        return invoices;
    }
}