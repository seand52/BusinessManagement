using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllSalesOrdersQuery: IRequest<PagedList<SalesOrderDto>>
{
    public PaginationFilter Filter;
    public SearchParams? SearchParams;
    public string UserId;

    public GetAllSalesOrdersQuery(PaginationFilter filter, SearchParams? searchParams, string userId)
    {
        Filter = filter;
        SearchParams = searchParams;
        UserId = userId;
    }
}