using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateClientHandler: IRequestHandler<CreateClientRequest, ClientDto> 
{
    private readonly IClientRepository _clientRepository;

    public CreateClientHandler (IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<ClientDto> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var clientEntity = request.Client.ToModel();
        clientEntity.UserId = request.UserId;
        await _clientRepository.InsertClient(clientEntity);
        await _clientRepository.Save();
        return clientEntity.ToDto();
    }
}
