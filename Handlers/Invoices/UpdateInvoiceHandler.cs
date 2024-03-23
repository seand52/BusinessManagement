using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateInvoiceHandler: IRequestHandler<UpdateInvoiceRequest, bool>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public UpdateInvoiceHandler (IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task<bool> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoiceEntity = request.Invoice.ToModel();
        var invoiceToUpdate = await _invoiceRepository.GetInvoiceById(request.Id, request.UserId);
        
        if (invoiceToUpdate == null)
        {
            throw new Exception("Invoice not found");
        }
        
        if (invoiceToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _invoiceRepository.UpdateInvoice(invoiceToUpdate, invoiceEntity);
        await _invoiceRepository.Save();
        return true;
    }
}