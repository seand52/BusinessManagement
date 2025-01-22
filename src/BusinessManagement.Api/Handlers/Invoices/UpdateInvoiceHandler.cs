using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class UpdateInvoiceHandler: IRequestHandler<UpdateInvoiceRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInvoiceHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoiceEntity = request.Invoice.ToModel();
        invoiceEntity.TotalPrice = invoiceEntity.CalculateTotalPrice();
        var invoiceToUpdate = await _unitOfWork.InvoiceRepository.GetBy(request.Id, request.UserId);
        
        if (invoiceToUpdate == null)
        {
            throw new Exception("Invoice not found");
        }
        
        if (invoiceToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _unitOfWork.InvoiceRepository.Update(invoiceToUpdate, invoiceEntity);
        await _unitOfWork.Save();
        return true;
    }
}