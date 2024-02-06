using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Commands;

public class UpdateProductRequest: IRequest<bool>
{
    public UpdateProductDto Product { get; }
    public int Id { get; }
    
    public string UserId { get; }
    
    public UpdateProductRequest(UpdateProductDto product, int id, string userId)
    {
        Product = product;
        Id = id;
        UserId = userId;
    }
}