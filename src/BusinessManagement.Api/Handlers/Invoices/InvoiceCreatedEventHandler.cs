using BusinessManagement.DAL;
using BusinessManagement.Helpers;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions.Events;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class InvoiceCreatedEventHandler : IRequestHandler<InvoiceCreatedEvent, byte[]>
{
    private IInvoiceDocumentBuilder _builder;
    private IUnitOfWork _unitOfWork;
    private readonly IAwsPublisher _awsPublisher;


    public InvoiceCreatedEventHandler (IInvoiceDocumentBuilder builder, IUnitOfWork unitOfWork, IAwsPublisher awsPublisher)
    {
        _builder = builder;
        _unitOfWork = unitOfWork;
        _awsPublisher = awsPublisher;
    }

    public async Task<byte[]> Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
    {
        var invoice = notification.Invoice;
        var businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(item => item.UserId == invoice.UserId);
        var documentBuilder = _builder.Build();
        documentBuilder.CreateInvoiceDocument(invoice, businessInfo.ToDto());
        var pdfBytes = documentBuilder.GeneratePdf();
        using var memoryStream = new MemoryStream(pdfBytes);
        await _awsPublisher.Publish($"invoices/{invoice.Id}", memoryStream);
        return pdfBytes;
    }
}