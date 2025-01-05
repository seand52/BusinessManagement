using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagementApi.Extensions.Events;

public class SalesOrderCreatedEvent : INotification
{
    public SalesOrderDetailDto SalesOrder { get; private set; }

    public SalesOrderCreatedEvent(SalesOrderDetailDto salesOrder)
    {
        SalesOrder = salesOrder;
    }
}