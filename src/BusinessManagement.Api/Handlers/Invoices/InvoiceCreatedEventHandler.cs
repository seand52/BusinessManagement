using BusinessManagement.DAL;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions.Events;
using BusinessManagementApi.Models;
using MediatR;
namespace BusinessManagement.Handlers;

public class InvoiceCreatedEventHandler : INotificationHandler<InvoiceCreatedEvent>
{
    private IInvoiceDocumentBuilder _builder;
    private IUnitOfWork _unitOfWork;
    
    
    public InvoiceCreatedEventHandler (IInvoiceDocumentBuilder builder, IUnitOfWork unitOfWork)
    {
        _builder = builder;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
    {
        var invoice = notification.Invoice;
        var businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(item => item.UserId == invoice.UserId);
        var documentBuilder = _builder.Build();
        documentBuilder.CreateInvoiceDocument(invoice, businessInfo.ToDto()).GeneratePdf($"invoices/user_{invoice.UserId}_invoice_{invoice.Id}.pdf");
    }
}