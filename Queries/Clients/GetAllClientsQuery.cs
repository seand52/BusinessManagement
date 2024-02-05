using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllClientsQuery: IRequest<PagedList<Client>>
{
    public PaginationFilter Filter;
    public string SearchTerm;
    public string UserId;

    public GetAllClientsQuery(PaginationFilter filter, string searchTerm, string userId)
    {
        Filter = filter;
        SearchTerm = searchTerm;
        UserId = userId;
    }
}