using MediatR;

namespace BusinessManagement.Commands;

public class DeleteSalesOrderRequest: IRequest<bool>
{
    public int Id { get; }
    public string UserId { get; }
    
    public DeleteSalesOrderRequest(int id, string userId)
    {
        Id = id;
        UserId = userId;
    }
    
}