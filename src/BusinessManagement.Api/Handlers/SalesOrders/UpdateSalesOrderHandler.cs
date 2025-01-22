using BusinessManagement.Commands;
using BusinessManagement.Commands.SalesOrders;
using BusinessManagement.DAL;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateSalesOrderHandler: IRequestHandler<UpdateSalesOrderRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSalesOrderHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(UpdateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrderEntity = request.SalesOrder.ToModel();
        salesOrderEntity.TotalPrice = salesOrderEntity.CalculateTotalPrice();
        var salesOrderToUpdate = await _unitOfWork.SalesOrderRepository.GetBy(request.Id, request.UserId);
        
        if (salesOrderToUpdate == null)
        {
            throw new Exception("SalesOrder not found");
        }
        
        if (salesOrderToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _unitOfWork.SalesOrderRepository.Update(salesOrderToUpdate, salesOrderEntity);
        await _unitOfWork.Save();
        return true;
    }
}