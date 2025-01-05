using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Commands;

public class UpdateClientRequest: IRequest<bool>
{
    public UpdateClientDto Client { get; }
    public int Id { get; }
    
    public string UserId { get; }
    
    public UpdateClientRequest(UpdateClientDto client, int id, string userId)
    {
        Client = client;
        Id = id;
        UserId = userId;
    }
}