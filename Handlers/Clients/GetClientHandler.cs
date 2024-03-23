using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetClientHandler: IRequestHandler<GetClientQuery, ClientDto?>
{
    private readonly IClientRepository _clientRepository;

    public GetClientHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task<ClientDto?> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        Client? client = await _clientRepository.GetClientById(request.ClientId, request.UserId);
        return client.ToDto();
    }
}  