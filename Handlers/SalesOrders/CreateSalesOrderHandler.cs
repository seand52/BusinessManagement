using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateSalesOrderHandler: IRequestHandler<CreateSalesOrderRequest, SalesOrderDetailDto> 
{
    private readonly ISalesOrderRepository _salesOrderRepository;
    private readonly IClientRepository _clientRepository;

    public CreateSalesOrderHandler (ISalesOrderRepository salesOrderRepository, IClientRepository clientRepository)
    {
        _salesOrderRepository = salesOrderRepository;
        _clientRepository = clientRepository;
    }
    public async Task<SalesOrderDetailDto> Handle(CreateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = request.SalesOrder.ToModel();
        salesOrder.UserId = request.UserId;
        salesOrder.TotalPrice = salesOrder.CalculateTotalPrice();
        await _salesOrderRepository.InsertSalesOrder(salesOrder);
        await _salesOrderRepository.Save();
        // TODO: find a better way of returning the client
        var client = await _clientRepository.GetClientById(salesOrder.ClientId, request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        salesOrder.Client = client;
        return salesOrder.ToDto();
    }
}