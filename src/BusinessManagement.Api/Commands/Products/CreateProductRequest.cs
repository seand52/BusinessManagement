using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Commands;

public class CreateProductRequest: IRequest<ProductDto>
{
    public CreateProductDto Product { get; }
    public string UserId { get; }
    
    public CreateProductRequest(CreateProductDto product, string userId)
    {
        Product = product;
        UserId = userId;
    }
}