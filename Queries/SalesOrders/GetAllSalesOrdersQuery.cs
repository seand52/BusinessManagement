using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllSalesOrdersQuery: IRequest<PagedList<SalesOrderDto>>
{
    public PaginationFilter Filter;
    public string SearchTerm;
    public string UserId;

    public GetAllSalesOrdersQuery(PaginationFilter filter, string searchTerm, string userId)
    {
        Filter = filter;
        SearchTerm = searchTerm;
        UserId = userId;
    }
}