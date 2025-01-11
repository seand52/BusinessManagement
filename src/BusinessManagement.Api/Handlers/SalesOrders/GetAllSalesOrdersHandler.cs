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
    private Expression<Func<SalesOrder, bool>>?  BuildSearchTerm(GetAllSalesOrdersQuery request)
    {
        if (request.SearchTerm == null)
        {
            return null;
        }

        return p => p.Client.Name.ToLower().Contains(request.SearchTerm.ToLower());
    }
    public async Task<PagedList<SalesOrderDto>> Handle(GetAllSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var validFilter = new PaginationFilter(request.Filter.PageNumber, request.Filter.PageSize);
        var searchTerm = BuildSearchTerm(request);
        var salesOrders = await _unitOfWork.SalesOrderRepository.GetAllBy(p => p.UserId  == request.UserId, validFilter, searchTerm, "Client");
        var salesOrderDtos = salesOrders.Items.Select(p => p.ToDto()).ToList();
        return new PagedList<SalesOrderDto>(salesOrderDtos, salesOrders.Page, salesOrders.PageSize, salesOrders.TotalCount);
    }
}