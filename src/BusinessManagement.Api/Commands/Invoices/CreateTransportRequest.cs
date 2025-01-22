using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class CreateTransportRequest: IRequest<InvoiceDetailDto>
{
    public CreateInvoiceDto Invoice { get; }
    public string UserId { get; }
    
    public CreateTransportRequest(CreateInvoiceDto invoice, string userId)
    {
        Invoice = invoice;
        UserId = userId;
    }
}