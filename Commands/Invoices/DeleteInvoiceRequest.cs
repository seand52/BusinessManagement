using MediatR;

namespace BusinessManagement.Commands;

public class DeleteInvoiceRequest: IRequest<bool>
{
    public int Id { get; }
    public string UserId { get; }
    
    public DeleteInvoiceRequest(int id, string userId)
    {
        Id = id;
        UserId = userId;
    }
    
}