using BusinessManagementApi.Dto;
using MediatR;

namespace BusinessManagement.Queries;

public class GetInvoiceQuery: IRequest<InvoiceDetailDto>
{
    public readonly int InvoiceId;
    public readonly string UserId;

    public GetInvoiceQuery(int invoiceId, string userId)
    {
        InvoiceId = invoiceId;
        UserId = userId;
    }
}