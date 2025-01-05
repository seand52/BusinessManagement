using MediatR;

namespace BusinessManagement.Commands;

public class DeleteProductRequest: IRequest<bool>
{
    public int Id { get; }
    public string UserId { get; }
    
    public DeleteProductRequest(int id, string userId)
    {
        Id = id;
        UserId = userId;
    }
    
}