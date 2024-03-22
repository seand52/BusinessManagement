using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Dto;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class CreateInvoiceHandler: IRequestHandler<CreateInvoiceRequest, InvoiceDetailDto> 
{
    private readonly IMapper _mapper;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IClientRepository _clientRepository;

    public CreateInvoiceHandler (IInvoiceRepository invoiceRepository, IClientRepository clientRepository, IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _clientRepository = clientRepository;
        _mapper = mapper;
    }
    public async Task<InvoiceDetailDto> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = request.Invoice.ToModel();
        invoice.UserId = request.UserId;
        //TODO: calculate total price
        invoice.TotalPrice = 50;
        await _invoiceRepository.InsertInvoice(invoice);
        await _invoiceRepository.Save();
        // TODO: find a better way of returning the client
        var client = await _clientRepository.GetClientById(invoice.ClientId, request.UserId);
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        invoice.Client = client;
        var test = _mapper.Map<InvoiceDetailDto>(invoice);
        return test;
    }
}