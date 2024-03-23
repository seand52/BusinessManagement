using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateProductHandler: IRequestHandler<CreateProductRequest, ProductDto> 
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler (IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ProductDto> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = request.Product.ToModel();
        productEntity.UserId = request.UserId;
        await _productRepository.InsertProduct(productEntity);
        await _productRepository.Save();
        return productEntity.ToDto();
    }
}
