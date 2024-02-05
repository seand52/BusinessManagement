using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateClientHandler: IRequestHandler<UpdateClientRequest, bool>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public UpdateClientHandler (IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var clientEntity = _mapper.Map<Client>(request.Client);
        var clientToUpdate = await _clientRepository.GetClientById(request.Id, request.UserId);
        
        if (clientToUpdate == null)
        {
            throw new Exception("Client not found");
        }
        
        if (clientToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _clientRepository.UpdateClient(clientToUpdate, clientEntity);
        await _clientRepository.Save();
        return true;
    }
}