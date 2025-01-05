using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class UpdateSalesOrderRequest: IRequest<bool>
{
    public UpdateSalesOrderDto SalesOrder { get; }
    public int Id { get; }
    
    public string UserId { get; }
    
    public UpdateSalesOrderRequest(UpdateSalesOrderDto salesOrder, int id, string userId)
    {
        SalesOrder = salesOrder;
        Id = id;
        UserId = userId;
    }
}