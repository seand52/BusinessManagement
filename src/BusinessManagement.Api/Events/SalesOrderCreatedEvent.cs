using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagementApi.Extensions.Events;

public class SalesOrderCreatedEvent : IRequest<byte[]>
{
    public SalesOrderDetailDto SalesOrder { get; private set; }

    public SalesOrderCreatedEvent(SalesOrderDetailDto salesOrder)
    {
        SalesOrder = salesOrder;
    }
}