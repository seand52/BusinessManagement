using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeleteProductHandler: IRequestHandler<DeleteProductRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetBy(p => p.Id == request.Id && p.UserId == request.UserId);

        if (product == null)
        {
            return false;
        }

        if (product.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.Save();
        return true;
    }
}