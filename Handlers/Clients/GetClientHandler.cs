using AutoMapper;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetClientHandler: IRequestHandler<GetClientQuery, ClientDto?>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public GetClientHandler(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }
    
    public async Task<ClientDto?> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        Client? client = await _clientRepository.GetClientById(request.ClientId, request.UserId);
        return _mapper.Map<ClientDto>(client);
    }
}  