using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateProductHandler: IRequestHandler<UpdateProductRequest, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler (IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = _mapper.Map<Product>(request.Product);
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