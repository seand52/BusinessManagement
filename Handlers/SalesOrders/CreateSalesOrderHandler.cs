using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateSalesOrderHandler: IRequestHandler<CreateSalesOrderRequest, SalesOrderDetailDto> 
{
    private readonly IMapper _mapper;
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IClientRepository _clientRepository;

    public CreateSalesOrderHandler (ISalesOrderRepository salesOrderRepository, IClientRepository clientRepository, IMapper mapper)
    {
        _salesOrderRepository = salesOrderRepository;
        _clientRepository = clientRepository;
        _mapper = mapper;
    }
    public async Task<SalesOrderDetailDto> Handle(CreateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = _mapper.Map<SalesOrder>(request.SalesOrder);
        salesOrder.UserId = request.UserId;
        //TODO: calculate total price
        salesOrder.TotalPrice = 50;
        await _salesOrderRepository.InsertSalesOrder(salesOrder);
        await _salesOrderRepository.Save();
        // TODO: find a better way of returning the client
        var client = await _clientRepository.GetClientById(salesOrder.ClientId, request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        salesOrder.Client = client;
        var test = _mapper.Map<SalesOrderDetailDto>(salesOrder);
        return test;
    }
}