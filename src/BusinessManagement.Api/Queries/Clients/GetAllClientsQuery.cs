using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetAllClientsQuery: IRequest<PagedList<ClientDto>>
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