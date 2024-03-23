using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllSalesOrdersHandler: IRequestHandler<GetAllSalesOrdersQuery, PagedList<SalesOrderDto>>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetAllSalesOrdersHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }
    public async Task<PagedList<SalesOrderDto>> Handle(GetAllSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var salesOrder = await _salesOrderRepository.GetSalesOrders(validFilter, request.SearchTerm, request.UserId);
        var salesOrderDtos = salesOrder.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<SalesOrderDto>(salesOrderDtos, salesOrder.TotalCount, salesOrder.Page, salesOrder.PageSize);
    }
}