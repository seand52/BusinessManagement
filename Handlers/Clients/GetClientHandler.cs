using BusinessManagement.DAL;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using ContosoUniversity.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetClientHandler: IRequestHandler<GetClientQuery, ClientDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ClientDto?> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        Client? client  = await _unitOfWork.ClientRepository.GetBy(p => p.UserId == request.UserId && p.Id == request.ClientId);
        return client.ToDto();
    }
}  