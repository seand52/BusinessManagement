using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetBusinessInfoQuery: IRequest<BusinessInfoDto>
{
    public readonly string UserId;

    public GetBusinessInfoQuery(string userId)
    {
        UserId = userId;
    }
}