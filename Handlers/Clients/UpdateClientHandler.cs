using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateClientHandler: IRequestHandler<UpdateClientRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var clientEntity = request.Client.ToModel();
        var clientToUpdate = await _unitOfWork.ClientRepository.GetBy( p=> p.Id == request.Id && p.UserId == request.UserId);
        
        if (clientToUpdate == null)
        {
            throw new Exception("Client not found");
        }
        
        if (clientToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        clientEntity.Id = clientToUpdate.Id;
        clientEntity.UserId = clientToUpdate.UserId;
        _unitOfWork.ClientRepository.Update(clientEntity);
        await _unitOfWork.Save();
        return true;
    }
}