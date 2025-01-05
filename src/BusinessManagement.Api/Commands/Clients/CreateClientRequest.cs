using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class CreateClientRequest: IRequest<ClientDto>
{
    public CreateClientDto Client { get; }
    public string UserId { get; }
    
    public CreateClientRequest(CreateClientDto client, string userId)
    {
        Client = client;
        UserId = userId;
    }
}