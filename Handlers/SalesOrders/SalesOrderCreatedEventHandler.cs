using BusinessManagement.DAL;
using BusinessManagement.Templates;
using BusinessManagementApi.Extensions.Events;
using BusinessManagementApi.Models;
using MediatR;
namespace BusinessManagement.Handlers;

public class SalesOrderCreatedEventHandler : INotificationHandler<SalesOrderCreatedEvent>
{
    private ISalesOrderBuilder _builder;
    private IUnitOfWork _unitOfWork;
    
    
    public SalesOrderCreatedEventHandler (ISalesOrderBuilder builder, IUnitOfWork unitOfWork)
    {
        _builder = builder;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SalesOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var salesOrder = notification.SalesOrder;
        var businessInfo = await _unitOfWork.BusinessInfoRepository.GetBy(item => item.UserId == salesOrder.UserId);
        var documentBuilder = _builder.Build();
        documentBuilder.CreateSalesOrder(salesOrder, businessInfo.ToDto()).GeneratePdf($"salesOrders/user_{salesOrder.UserId}_salesOrder_{salesOrder.Id}.pdf");
    }
}