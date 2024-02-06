using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateProductHandler: IRequestHandler<CreateProductRequest, ProductDto> 
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public CreateProductHandler (IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<ProductDto> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = _mapper.Map<Product>(request.Product);
        productEntity.UserId = request.UserId;
        await _productRepository.InsertProduct(productEntity);
        await _productRepository.Save();
        return _mapper.Map<ProductDto>(productEntity);
    }
}
