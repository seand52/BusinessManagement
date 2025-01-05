using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetSalesOrderQuery: IRequest<SalesOrderDetailDto>
{
    public readonly int SalesOrderId;
    public readonly string UserId;

    public GetSalesOrderQuery(int salesOrderId, string userId)
    {
        SalesOrderId = salesOrderId;
        UserId = userId;
    }
}