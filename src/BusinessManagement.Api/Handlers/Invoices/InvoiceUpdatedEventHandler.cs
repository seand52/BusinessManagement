using BusinessManagement.DAL;
using BusinessManagement.Helpers;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions.Events;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class InvoiceUpdatedEventHandler : IRequestHandler<InvoiceUpdatedEvent>
{
    private IInvoiceDocumentBuilder _builder;
    private IUnitOfWork _unitOfWork;
    private readonly IAwsPublisher _awsPublisher;


    public InvoiceUpdatedEventHandler (IInvoiceDocumentBuilder builder, IUnitOfWork unitOfWork, IAwsPublisher awsPublisher)
    {
        _builder = builder;
        _unitOfWork = unitOfWork;
        _awsPublisher = awsPublisher;
    }

    public async Task Handle(InvoiceUpdatedEvent request, CancellationToken cancellationToken)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetBy(request.InvoiceId, request.UserId);
        var businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(item => item.UserId == invoice.UserId);
        var documentBuilder = _builder.Build();
        documentBuilder.CreateInvoiceDocument(invoice.ToDetailDto(), businessInfo.ToDto());
        var pdfBytes = documentBuilder.GeneratePdf();
        using var memoryStream = new MemoryStream(pdfBytes);
        await _awsPublisher.Publish($"invoices/{invoice.Id}", memoryStream);
    }
}