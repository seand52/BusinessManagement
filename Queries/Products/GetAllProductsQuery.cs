using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllProductsQuery: IRequest<PagedList<Product>>
{
    public PaginationFilter Filter;
    public string SearchTerm;
    public string UserId;

    public GetAllProductsQuery(PaginationFilter filter, string searchTerm, string userId)
    {
        Filter = filter;
        SearchTerm = searchTerm;
        UserId = userId;
    }
}