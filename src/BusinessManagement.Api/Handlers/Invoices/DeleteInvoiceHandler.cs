using BusinessManagement.Commands;
using BusinessManagement.DAL;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class DeleteInvoiceHandler: IRequestHandler<DeleteInvoiceRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteInvoiceHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetBy(request.Id, request.UserId);

        if (invoice == null)
        {
            return false;
        }

        if (invoice.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }

        _unitOfWork.InvoiceRepository.Delete(invoice);
        await _unitOfWork.Save();
        return true;
    }
}