using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagementApi.Extensions.Events;

public class InvoiceCreatedEvent : IRequest<byte[]>
{
    public InvoiceDetailDto Invoice { get; private set; }

    public InvoiceCreatedEvent(InvoiceDetailDto invoice)
    {
        Invoice = invoice;
    }
}