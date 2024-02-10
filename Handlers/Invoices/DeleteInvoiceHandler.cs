using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using MediatR;

namespace BusinessManagement.Handlers;

public class DeletInvoiceHandler: IRequestHandler<DeleteInvoiceRequest, bool>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public DeletInvoiceHandler (IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task<bool> Handle(DeleteInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(request.Id, request.UserId);

        if (invoice == null)
        {
            return false;
        }

        if (invoice.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _invoiceRepository.DeleteInvoice(invoice);
        await _invoiceRepository.Save();
        return true;
    }
}