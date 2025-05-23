using System.Linq.Expressions;
using BusinessManagement.DAL;
using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetAllSalesOrdersHandler: IRequestHandler<GetAllSalesOrdersQuery, PagedList<SalesOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSalesOrdersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PagedList<SalesOrderDto>> Handle(GetAllSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var salesOrders = await _unitOfWork.SalesOrderRepository.GetAllBy(request.UserId, validFilter, request.SearchParams);
        var salesOrderDtos = salesOrders.Items.Select(p => p.ToDto()).ToList();
        return new PagedList<SalesOrderDto>(salesOrderDtos, salesOrders.Page, salesOrders.PageSize, salesOrders.TotalCount);
    }
}