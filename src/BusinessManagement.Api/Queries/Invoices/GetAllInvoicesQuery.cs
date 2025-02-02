using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllInvoicesQuery: IRequest<PagedList<InvoiceDto>>
{
    public readonly PaginationFilter Filter;
    public readonly SearchParams? SearchParams;
    public readonly string UserId;

    public GetAllInvoicesQuery(PaginationFilter filter, SearchParams? searchParams, string userId)
    {
        Filter = filter;
        SearchParams = searchParams;
        UserId = userId;
    }
}