using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeleteClientHandler: IRequestHandler<DeleteClientRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.ClientRepository.GetBy(p => p.Id == request.Id && p.UserId == request.UserId);

        if (client == null)
        {
            return false;
        }

        if (client.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _unitOfWork.ClientRepository.Delete(client);
        await _unitOfWork.Save();
        return true;
    }
}