using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateProductHandler: IRequestHandler<CreateProductRequest, ProductDto> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ProductDto> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = request.Product.ToModel();
        productEntity.UserId = request.UserId;
        await _unitOfWork.ProductRepository.Insert(productEntity);
        await _unitOfWork.Save();
        return productEntity.ToDto();
    }
}
