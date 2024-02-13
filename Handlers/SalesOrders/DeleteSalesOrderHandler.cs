using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeletSalesOrderHandler: IRequestHandler<DeleteSalesOrderRequest, bool>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public DeletSalesOrderHandler (ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }
    public async Task<bool> Handle(DeleteSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = await _salesOrderRepository.GetSalesOrderById(request.Id, request.UserId);

        if (salesOrder == null)
        {
            return false;
        }

        if (salesOrder.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _salesOrderRepository.DeleteSalesOrder(salesOrder);
        await _salesOrderRepository.Save();
        return true;
    }
}