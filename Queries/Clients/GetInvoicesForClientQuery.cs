using BusinessManagement.Filter;
using BusinessManagement.Helpers;
using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetInvoicesForClientQuery: IRequest<PagedList<InvoiceDto>>
{
    public int ClientId { get; }
    public string UserId { get; }
    public PaginationFilter Filter;

    public GetInvoicesForClientQuery(int clientId, string userId, PaginationFilter filter)
    {
        ClientId = clientId;
        UserId = userId;
        Filter = filter;
    }
    
}