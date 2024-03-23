using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateProductHandler: IRequestHandler<UpdateProductRequest, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler (IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = request.Product.ToModel();
        var clientToUpdate = await _productRepository.GetProductById(request.Id, request.UserId);
        
        if (clientToUpdate == null)
        {
            throw new Exception("Product not found");
        }
        
        if (clientToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _productRepository.UpdateProduct(clientToUpdate, productEntity);
        await _productRepository.Save();
        return true;
    }
}