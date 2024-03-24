using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateInvoiceHandler: IRequestHandler<CreateInvoiceRequest, InvoiceDetailDto> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<InvoiceDetailDto> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = request.Invoice.ToModel();
        invoice.UserId = request.UserId;
        invoice.TotalPrice = invoice.CalculateTotalPrice();
        await _unitOfWork.InvoiceRepository.Insert(invoice);
        await _unitOfWork.Save();
        // TODO: find a better way of returning the client
        var client = await _unitOfWork.ClientRepository.GetBy(p => p.Id == invoice.ClientId && p.UserId == request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        invoice.Client = client;
        return invoice.ToDetailDto();
    }
}