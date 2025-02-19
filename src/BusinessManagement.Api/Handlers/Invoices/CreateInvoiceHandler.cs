using BusinessManagement.Commands;
using BusinessManagement.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers.Invoices;

public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceRequest, InvoiceDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private Invoice GetTransportInvoice(CreateInvoiceRequest request)
    {
        var invoice = request.Invoice;
        var transportInvoice = invoice.ToTransportOnly();
        transportInvoice.UserId = request.UserId;
        transportInvoice.TotalPrice = transportInvoice.CalculateTotalPrice();
        transportInvoice.DateIssued = request.Invoice.DateIssued.ToUniversalTime();
        return transportInvoice;
    }

    public async Task<InvoiceDetailDto> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = request.Invoice.ToModel();
        invoice.UserId = request.UserId;
        invoice.TotalPrice = invoice.CalculateTotalPrice();
        invoice.DateIssued = request.Invoice.DateIssued.ToUniversalTime();
        await _unitOfWork.InvoiceRepository.Insert(invoice, request.UserId);
        if (invoice.TransportPrice != 0)
        {
            var transportInvoice = GetTransportInvoice(request);
            await _unitOfWork.InvoiceRepository.Insert(transportInvoice, request.UserId);
        }

        await _unitOfWork.Save();
        // TODO: find a better way of returning the client
        var client =
            await _unitOfWork.ClientRepository.GetBy(p => p.Id == invoice.ClientId && p.UserId == request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }

        invoice.Client = client;
        return invoice.ToDetailDto();
    }
}