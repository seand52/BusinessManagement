using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands.SalesOrders;

public class ConvertSalesOrderToInvoiceRequest : IRequest<InvoiceDetailDto?>
{
    public int SalesOrderId { get; }
    public string UserId { get; }

    public ConvertSalesOrderToInvoiceRequest(int salesOrderId, string userId)
    {
        SalesOrderId = salesOrderId;
        UserId = userId;
    }
}