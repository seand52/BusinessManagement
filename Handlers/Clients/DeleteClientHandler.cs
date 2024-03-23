using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeleteClientHandler: IRequestHandler<DeleteClientRequest, bool>
{
    private readonly IClientRepository _clientRepository;

    public DeleteClientHandler (IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<bool> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientById(request.Id, request.UserId);

        if (client == null)
        {
            return false;
        }

        if (client.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _clientRepository.DeleteClient(client);
        await _clientRepository.Save();
        return true;
    }
}