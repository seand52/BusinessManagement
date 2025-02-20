using BusinessManagement.Commands;
using BusinessManagement.Commands.SalesOrders;
using BusinessManagement.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateSalesOrderHandler: IRequestHandler<CreateSalesOrderRequest, SalesOrderDetailDto> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSalesOrderHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    private SalesOrder GetTransportSalesOrder(CreateSalesOrderRequest request)
    {
        var salesOrder = request.SalesOrder;
        var transportSalesOrder = salesOrder.ToTransportOnly();
        transportSalesOrder.UserId = request.UserId;
        transportSalesOrder.TotalPrice = transportSalesOrder.CalculateTotalPrice();
        transportSalesOrder.DateIssued = request.SalesOrder.DateIssued.ToUniversalTime();
        return transportSalesOrder;
    }
    
    public async Task<SalesOrderDetailDto> Handle(CreateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = request.SalesOrder.ToModel();
        salesOrder.UserId = request.UserId;
        salesOrder.TotalPrice = salesOrder.CalculateTotalPrice();
        salesOrder.DateIssued = salesOrder.DateIssued.ToUniversalTime();
        await _unitOfWork.SalesOrderRepository.Insert(salesOrder, request.UserId);
        if (salesOrder.TransportPrice != 0)
        {
            var transportInvoice = GetTransportSalesOrder(request);
            await _unitOfWork.SalesOrderRepository.Insert(transportInvoice, request.UserId);
        }
        await _unitOfWork.Save();
        var client = await _unitOfWork.ClientRepository.GetBy(p => p.Id == salesOrder.ClientId && p.UserId == request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        salesOrder.Client = client;
        return salesOrder.ToDetailDto();
    }
}