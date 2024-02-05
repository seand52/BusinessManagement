using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllClientsHandler: IRequestHandler<GetAllClientsQuery, PagedList<Client>>
{
    private readonly IClientRepository _clientRepository;

    public GetAllClientsHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<PagedList<Client>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var clients = await _clientRepository.GetClients(validFilter, request.SearchTerm, request.UserId);
        return clients;
    }
}