using MediatR;

namespace BusinessManagement.Commands;

public class ConvertSalesOrderToInvoiceRequest : IRequest<bool>
{
    public int SalesOrderId { get; }
    public string UserId { get; }

    public ConvertSalesOrderToInvoiceRequest(int salesOrderId, string userId)
    {
        SalesOrderId = salesOrderId;
        UserId = userId;
    }
}