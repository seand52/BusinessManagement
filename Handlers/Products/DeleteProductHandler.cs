using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeleteProductHandler: IRequestHandler<DeleteProductRequest, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler (IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(request.Id, request.UserId);

        if (product == null)
        {
            return false;
        }

        if (product.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _productRepository.DeleteProduct(product);
        await _productRepository.Save();
        return true;
    }
}