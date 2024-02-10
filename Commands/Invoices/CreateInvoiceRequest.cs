using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class CreateInvoiceRequest: IRequest<InvoiceDetailDto>
{
    public CreateInvoiceDto Invoice { get; }
    public string UserId { get; }
    
    public CreateInvoiceRequest(CreateInvoiceDto invoice, string userId)
    {
        Invoice = invoice;
        UserId = userId;
    }
}