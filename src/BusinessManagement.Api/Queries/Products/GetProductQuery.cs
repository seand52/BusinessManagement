using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetProductQuery: IRequest<ProductDto>
{
    public readonly int ProductId;
    public readonly string UserId;

    public GetProductQuery(int clientId, string userId)
    {
        ProductId = clientId;
        UserId = userId;
    }
}