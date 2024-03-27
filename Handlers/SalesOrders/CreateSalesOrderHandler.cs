using BusinessManagement.Commands;
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
    public async Task<SalesOrderDetailDto> Handle(CreateSalesOrderRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = request.SalesOrder.ToModel();
        salesOrder.UserId = request.UserId;
        salesOrder.TotalPrice = salesOrder.CalculateTotalPrice();
        salesOrder.DateIssued = salesOrder.DateIssued.ToUniversalTime();
        await _unitOfWork.SalesOrderRepository.Insert(salesOrder);
        await _unitOfWork.Save();
        // TODO: find a better way of returning the client
        var client = await _unitOfWork.ClientRepository.GetBy(p => p.Id == salesOrder.ClientId && p.UserId == request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        salesOrder.Client = client;
        return salesOrder.ToDetailDto();
    }
}