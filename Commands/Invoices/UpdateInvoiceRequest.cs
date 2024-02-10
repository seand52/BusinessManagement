using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class UpdateInvoiceRequest: IRequest<bool>
{
    public UpdateInvoiceDto Invoice { get; }
    public int Id { get; }
    
    public string UserId { get; }
    
    public UpdateInvoiceRequest(UpdateInvoiceDto invoice, int id, string userId)
    {
        Invoice = invoice;
        Id = id;
        UserId = userId;
    }
}