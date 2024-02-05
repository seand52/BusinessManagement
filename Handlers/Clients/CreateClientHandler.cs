using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateClientHandler: IRequestHandler<CreateClientRequest, ClientDto> 
{
    private readonly IMapper _mapper;
    private readonly IClientRepository _clientRepository;

    public CreateClientHandler (IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }
    public async Task<ClientDto> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var clientEntity = _mapper.Map<Client>(request.Client);
        clientEntity.UserId = request.UserId;
        await _clientRepository.InsertClient(clientEntity);
        await _clientRepository.Save();
        return _mapper.Map<ClientDto>(clientEntity);
    }
}
