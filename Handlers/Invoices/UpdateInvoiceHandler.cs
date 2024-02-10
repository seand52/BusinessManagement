using AutoMapper;
using BusinessManagement.Commands;
using BusinessManagementApi.DAL;
using BusinessManagementApi.Models;
using MediatR;

namespace BusinessManagement.Handlers;

public class UpdateInvoiceHandler: IRequestHandler<UpdateInvoiceRequest, bool>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public UpdateInvoiceHandler (IInvoiceRepository invoiceRepository, IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoiceEntity = _mapper.Map<Invoice>(request.Invoice);
        var invoiceToUpdate = await _invoiceRepository.GetInvoiceById(request.Id, request.UserId);
        
        if (invoiceToUpdate == null)
        {
            throw new Exception("Invoice not found");
        }
        
        if (invoiceToUpdate.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("Insufficient Permissions");
        }
        
        _invoiceRepository.UpdateInvoice(invoiceToUpdate, invoiceEntity);
        await _invoiceRepository.Save();
        return true;
    }
}