using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllSalesOrdersHandler: IRequestHandler<GetAllSalesOrdersQuery, PagedList<SalesOrder>>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetAllSalesOrdersHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }
    public async Task<PagedList<SalesOrder>> Handle(GetAllSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var salesOrder = await _salesOrderRepository.GetSalesOrders(validFilter, request.SearchTerm, request.UserId);
        return salesOrder;
    }
}