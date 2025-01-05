using MediatR;

namespace BusinessManagement.Commands;

public class DeleteClientRequest: IRequest<bool>
{
    public int Id { get; }
    public string UserId { get; }
    
    public DeleteClientRequest(int id, string userId)
    {
        Id = id;
        UserId = userId;
    }
    
}