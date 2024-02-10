using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllInvoicesQuery: IRequest<PagedList<Invoice>>
{
    public PaginationFilter Filter;
    public string SearchTerm;
    public string UserId;

    public GetAllInvoicesQuery(PaginationFilter filter, string searchTerm, string userId)
    {
        Filter = filter;
        SearchTerm = searchTerm;
        UserId = userId;
    }
}