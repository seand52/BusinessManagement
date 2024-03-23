using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetSalesOrderHandler: IRequestHandler<GetSalesOrderQuery, SalesOrderDetailDto?>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetSalesOrderHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }
    
    public async Task<SalesOrderDetailDto?> Handle(GetSalesOrderQuery request, CancellationToken cancellationToken)
    {
        SalesOrder? salesOrder = await _salesOrderRepository.GetSalesOrderById(request.SalesOrderId, request.UserId);
        return salesOrder.ToDto();
    }
}