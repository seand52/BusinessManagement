using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class CreateTransportInvoiceHandler: IRequestHandler<CreateTransportRequest, InvoiceDetailDto> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransportInvoiceHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<InvoiceDetailDto> Handle(CreateTransportRequest request, CancellationToken cancellationToken)
    {
        var transportOnlyInvoice = request.Invoice.ToTransportOnly();
        transportOnlyInvoice.UserId = request.UserId;
        transportOnlyInvoice.TotalPrice = transportOnlyInvoice.CalculateTotalPrice();
        transportOnlyInvoice.DateIssued = request.Invoice.DateIssued.ToUniversalTime();
        await _unitOfWork.InvoiceRepository.Insert(transportOnlyInvoice);
        await _unitOfWork.Save();
        var client = await _unitOfWork.ClientRepository.GetBy(p => p.Id == transportOnlyInvoice.ClientId && p.UserId == request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        transportOnlyInvoice.Client = client;
        return transportOnlyInvoice.ToDetailDto();
    }
}