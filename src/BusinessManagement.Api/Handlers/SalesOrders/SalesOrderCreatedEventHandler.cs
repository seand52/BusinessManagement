using BusinessManagement.DAL;
using BusinessManagement.Helpers;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions.Events;
using BusinessManagementApi.Models;
using MediatR;
namespace BusinessManagement.Handlers;

public class SalesOrderCreatedEventHandler : IRequestHandler<SalesOrderCreatedEvent, byte[]>
{
    private ISalesOrderBuilder _builder;
    private IUnitOfWork _unitOfWork;
    private readonly IAwsPublisher _awsPublisher;


    public SalesOrderCreatedEventHandler (ISalesOrderBuilder builder, IUnitOfWork unitOfWork, IAwsPublisher awsPublisher)
    {
        _builder = builder;
        _unitOfWork = unitOfWork;
        _awsPublisher = awsPublisher;
    }

    public async Task<byte[]> Handle(SalesOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var salesOrder = notification.SalesOrder;
        var businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(item => item.UserId == salesOrder.UserId);
        var documentBuilder = _builder.Build();
        documentBuilder.CreateSalesOrder(salesOrder, businessInfo.ToDto());
        var pdfBytes = documentBuilder.GeneratePdf();
        using var memoryStream = new MemoryStream(pdfBytes);
        await _awsPublisher.Publish($"salesOrders/{salesOrder.Id}", memoryStream);
        return pdfBytes;
    }
}