using AutoMapper;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetSalesOrderHandler: IRequestHandler<GetSalesOrderQuery, SalesOrderDetailDto?>
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IMapper _mapper;

    public GetSalesOrderHandler(ISalesOrderRepository salesOrderRepository, IMapper mapper)
    {
        _salesOrderRepository = salesOrderRepository;
        _mapper = mapper;
    }
    
    public async Task<SalesOrderDetailDto?> Handle(GetSalesOrderQuery request, CancellationToken cancellationToken)
    {
        SalesOrder? salesOrder = await _salesOrderRepository.GetSalesOrderById(request.SalesOrderId, request.UserId);
        return _mapper.Map<SalesOrderDetailDto>(salesOrder);
    }
}