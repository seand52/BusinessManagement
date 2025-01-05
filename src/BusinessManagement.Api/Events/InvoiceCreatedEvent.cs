using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagementApi.Extensions.Events;

public class InvoiceCreatedEvent : INotification
{
    public InvoiceDetailDto Invoice { get; private set; }

    public InvoiceCreatedEvent(InvoiceDetailDto invoice)
    {
        Invoice = invoice;
    }
}