using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class CreateSalesOrderRequest: IRequest<SalesOrderDetailDto>
{
    public CreateSalesOrderDto SalesOrder { get; }
    public string UserId { get; }
    
    public CreateSalesOrderRequest(CreateSalesOrderDto salesOrder, string userId)
    {
        SalesOrder = salesOrder;
        UserId = userId;
    }
}