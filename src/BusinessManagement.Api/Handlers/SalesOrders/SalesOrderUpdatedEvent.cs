using BusinessManagement.DAL;
using BusinessManagement.Helpers;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions.Events;
using BusinessManagementApi.Models;
using MediatR;
namespace BusinessManagement.Handlers;

public class SalesOrderUpdatedEventHandler : IRequestHandler<SalesOrderUpdatedEvent>
{
    private ISalesOrderBuilder _builder;
    private IUnitOfWork _unitOfWork;
    private readonly IAwsPublisher _awsPublisher;


    public SalesOrderUpdatedEventHandler (ISalesOrderBuilder builder, IUnitOfWork unitOfWork, IAwsPublisher awsPublisher)
    {
        _builder = builder;
        _unitOfWork = unitOfWork;
        _awsPublisher = awsPublisher;
    }

    public async Task Handle(SalesOrderUpdatedEvent request, CancellationToken cancellationToken)
    {
        var salesOrder = await _unitOfWork.SalesOrderRepository.GetBy(request.SalesOrderId, request.UserId);
        var businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(item => item.UserId == salesOrder.UserId);
        var documentBuilder = _builder.Build();
        documentBuilder.CreateSalesOrder(salesOrder.ToDetailDto(), businessInfo.ToDto());
        var pdfBytes = documentBuilder.GeneratePdf();
        using var memoryStream = new MemoryStream(pdfBytes);
        await _awsPublisher.Publish($"salesOrders/{salesOrder.Id}", memoryStream);
    }
}