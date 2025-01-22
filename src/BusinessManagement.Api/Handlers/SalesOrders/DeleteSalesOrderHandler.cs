using BusinessManagement.Commands;
using BusinessManagement.Commands.SalesOrders;
using BusinessManagement.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeletSalesOrderHandler: IRequestHandler<DeleteSalesOrderRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletSalesOrderHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = await _unitOfWork.SalesOrderRepository.GetBy(request.Id, request.UserId);

        if (salesOrder == null)
        {
            return false;
        }

        if (salesOrder.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _unitOfWork.SalesOrderRepository.Delete(salesOrder);
        await _unitOfWork.Save();
        return true;
    }
}