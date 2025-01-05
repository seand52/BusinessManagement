using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateProductHandler: IRequestHandler<UpdateProductRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = request.Product.ToModel();
        var productToUpdate = await _unitOfWork.ProductRepository.GetBy(p => p.Id == request.Id && p.UserId == request.UserId);
        
        if (productToUpdate == null)
        {
            throw new Exception("Product not found");
        }
        
        if (productToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        productEntity.Id = productToUpdate.Id;
        productEntity.UserId = productToUpdate.UserId;
        _unitOfWork.ProductRepository.Update(productEntity);
        await _unitOfWork.Save();
        return true;
    }
}