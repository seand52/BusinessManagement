using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetSalesOrdersForClientHandler: IRequestHandler<GetSalesOrdersForClientQuery, PagedList<SalesOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesOrdersForClientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PagedList<SalesOrderDto>> Handle(GetSalesOrdersForClientQuery request, CancellationToken cancellationToken)
    {
        var paginationFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var salesOrders = await _unitOfWork.SalesOrderRepository.GetAllBy(p => p.UserId == request.UserId && p.ClientId == request.ClientId, paginationFilter, null, "SalesOrderProducts");
        var salesOrdersDtos = salesOrders.Items.Select(item => item.ToDto()).ToList();
        return new PagedList<SalesOrderDto>(salesOrdersDtos, salesOrders.TotalCount, salesOrders.Page, salesOrders.PageSize);
    }
}