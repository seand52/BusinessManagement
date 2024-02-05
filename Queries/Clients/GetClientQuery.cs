using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetClientQuery: IRequest<ClientDto>
{
    public readonly int ClientId;
    public readonly string UserId;

    public GetClientQuery(int clientId, string userId)
    {
        ClientId = clientId;
        UserId = userId;
    }
}