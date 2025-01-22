using BusinessManagement.Commands;
using BusinessManagement.Commands.SalesOrders;
using BusinessManagement.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class ConvertSalesOrderToInvoiceHandler:  IRequestHandler<ConvertSalesOrderToInvoiceRequest, InvoiceDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConvertSalesOrderToInvoiceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InvoiceDetailDto?> Handle(ConvertSalesOrderToInvoiceRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = await _unitOfWork.SalesOrderRepository.GetBy(request.SalesOrderId, request.UserId);
        if (salesOrder == null)
        {
            return null;
        }

        if (salesOrder.Expired == 1)
        {
            return null;
        }
        var invoice = salesOrder.ToInvoice();
        await _unitOfWork.InvoiceRepository.Insert(invoice);
        
        salesOrder.Expired = 1;
        _unitOfWork.SalesOrderRepository.Update(salesOrder);
        
        await _unitOfWork.Save();
        // TODO: find a better way of returning the client
        var client =
            await _unitOfWork.ClientRepository.GetBy(p => p.Id == invoice.ClientId && p.UserId == request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }

        invoice.Client = client;
        return invoice.ToDetailDto();
    }
}