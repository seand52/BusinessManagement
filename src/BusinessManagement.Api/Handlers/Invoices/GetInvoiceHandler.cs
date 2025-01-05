using BusinessManagement.DAL;
using BusinessManagement.Queries;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class GetInvoiceHandler: IRequestHandler<GetInvoiceQuery, InvoiceDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInvoiceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<InvoiceDetailDto?> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await _unitOfWork.InvoiceRepository.GetBy(request.InvoiceId, request.UserId);
        return invoice.ToDetailDto();
    }
}