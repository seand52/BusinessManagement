using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class GetAllInvoicesHandler: IRequestHandler<GetAllInvoicesQuery, PagedList<InvoiceDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllInvoicesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PagedList<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var invoices = await _unitOfWork.InvoiceRepository.GetAllBy(request.UserId, validFilter, request.SearchParams);
        var invoiceDtos = invoices.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<InvoiceDto>(invoiceDtos, invoices.Page, invoices.PageSize, invoices.TotalCount);
    }
}