using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateSalesOrderHandler: IRequestHandler<UpdateSalesOrderRequest, bool>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public UpdateSalesOrderHandler (ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }
    public async Task<bool> Handle(UpdateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrderEntity = request.SalesOrder.ToModel();
        var salesOrderToUpdate = await _salesOrderRepository.GetSalesOrderById(request.Id, request.UserId);
        
        if (salesOrderToUpdate == null)
        {
            throw new Exception("SalesOrder not found");
        }
        
        if (salesOrderToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _salesOrderRepository.UpdateSalesOrder(salesOrderToUpdate, salesOrderEntity);
        await _salesOrderRepository.Save();
        return true;
    }
}