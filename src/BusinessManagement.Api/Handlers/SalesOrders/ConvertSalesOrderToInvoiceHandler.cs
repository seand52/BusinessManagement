using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class ConvertSalesOrderToInvoiceHandler:  IRequestHandler<ConvertSalesOrderToInvoiceRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConvertSalesOrderToInvoiceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ConvertSalesOrderToInvoiceRequest request, CancellationToken cancellationToken)
    {
        var salesOrder = await _unitOfWork.SalesOrderRepository.GetBy(request.SalesOrderId, request.UserId);
        if (salesOrder == null)
        {
            return false;
        }

        if (salesOrder.Expired == 1)
        {
            return false;
        }
        
        if (salesOrder.SalesOrderProducts.Count == 0)
        {
            return false;
        }

        var invoice = salesOrder.ToInvoice();
        await _unitOfWork.InvoiceRepository.Insert(invoice);
        
        salesOrder.Expired = 1;
        _unitOfWork.SalesOrderRepository.Update(salesOrder);
        
        await _unitOfWork.Save();
        return true;
    }
}