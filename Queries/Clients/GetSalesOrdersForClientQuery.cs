using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetSalesOrdersForClientQuery: IRequest<PagedList<SalesOrderDto>>
{
    public int ClientId { get; }
    public string UserId { get; }
    public PaginationFilter Filter;

    public GetSalesOrdersForClientQuery(int clientId, string userId, PaginationFilter filter)
    {
        ClientId = clientId;
        UserId = userId;
        Filter = filter;
    }
    
}