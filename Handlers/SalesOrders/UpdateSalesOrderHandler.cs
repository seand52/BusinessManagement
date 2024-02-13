using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateSalesOrderHandler: IRequestHandler<UpdateSalesOrderRequest, bool>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IMapper _mapper;

    public UpdateSalesOrderHandler (ISalesOrderRepository salesOrderRepository, IMapper mapper)
    {
        _salesOrderRepository = salesOrderRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrderEntity = _mapper.Map<SalesOrder>(request.SalesOrder);
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