using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetInvoicesForClientHandler: IRequestHandler<GetInvoicesForClientQuery, PagedList<InvoiceDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInvoicesForClientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PagedList<InvoiceDto>> Handle(GetInvoicesForClientQuery request, CancellationToken cancellationToken)
    {
        var paginationFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var invoices = await _unitOfWork.InvoiceRepository.GetAllBy(p => p.UserId == request.UserId && p.ClientId == request.ClientId, paginationFilter, null, "InvoiceProducts");
        var invoiceDtos = invoices.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<InvoiceDto>(invoiceDtos, invoices.TotalCount, invoices.Page, invoices.PageSize);
    }
}