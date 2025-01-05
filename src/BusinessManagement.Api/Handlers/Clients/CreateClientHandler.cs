using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateClientHandler: IRequestHandler<CreateClientRequest, ClientDto> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ClientDto> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var clientEntity = request.Client.ToModel();
        clientEntity.UserId = request.UserId;
        await _unitOfWork.ClientRepository.Insert(clientEntity);
        await _unitOfWork.Save();
        return clientEntity.ToDto();
    }
}
