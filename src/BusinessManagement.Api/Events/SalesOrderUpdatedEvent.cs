using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagementApi.Extensions.Events;

public class SalesOrderUpdatedEvent : IRequest
{
    public string UserId { get; private set; }
    public int SalesOrderId { get; private set; }

    public SalesOrderUpdatedEvent(int id, string userId)
    {
        UserId = userId;
        SalesOrderId = id;
    }
}