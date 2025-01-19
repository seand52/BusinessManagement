using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagementApi.Extensions.Events;

public class InvoiceUpdatedEvent : IRequest
{
    public int InvoiceId { get; private set; }
    public string UserId { get; private set; }

    public InvoiceUpdatedEvent(int id, string userId)
    {
        InvoiceId = id;
        UserId = userId;
    }
}