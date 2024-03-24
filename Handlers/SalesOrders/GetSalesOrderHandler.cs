using BusinessManagement.DAL;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetSalesOrderHandler: IRequestHandler<GetSalesOrderQuery, SalesOrderDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesOrderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SalesOrderDetailDto?> Handle(GetSalesOrderQuery request, CancellationToken cancellationToken)
    {
        SalesOrder? salesOrder = await _unitOfWork.SalesOrderRepository.GetBy(request.SalesOrderId, request.UserId);
        return salesOrder.ToDetailDto();
    }
}