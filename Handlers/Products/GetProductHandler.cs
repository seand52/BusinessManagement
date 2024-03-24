using BusinessManagement.DAL;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetProductHandler: IRequestHandler<GetProductQuery, ProductDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        Product? product = await _unitOfWork.ProductRepository.GetBy(p => p.Id == request.ProductId && p.UserId == request.UserId);
        return product.ToDto();
    }
}  