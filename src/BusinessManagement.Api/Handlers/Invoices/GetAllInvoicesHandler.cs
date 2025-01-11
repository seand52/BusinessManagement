using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllInvoicesHandler: IRequestHandler<GetAllInvoicesQuery, PagedList<InvoiceDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllInvoicesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    private Expression<Func<Invoice, bool>>?  BuildSearchTerm(GetAllInvoicesQuery request)
    {
        if (request.SearchTerm == null)
        {
            return null;
        }

        return p => p.Client.Name.ToLower().Contains(request.SearchTerm.ToLower());
    }
    public async Task<PagedList<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var searchTerm = BuildSearchTerm(request);
        var invoices = await _unitOfWork.InvoiceRepository.GetAllBy(p => p.UserId  == request.UserId, validFilter, searchTerm, "Client");
        var invoiceDtos = invoices.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<InvoiceDto>(invoiceDtos, invoices.Page, invoices.PageSize, invoices.TotalCount);
    }
}