using BusinessManagementApi.Dto;

namespace BusinessManagementApi.Models;

public static class ModelExtensions
{
    public static InvoiceDetailDto ToDto(this Invoice? invoice)
    {
        if (invoice is null)
        {
            return null;
        }
        return new InvoiceDetailDto
        {
            Id = invoice.Id,
            Re = invoice.Re,
            Tax = invoice.Tax,
            TransportPrice = invoice.TransportPrice,
            PaymentType = invoice.PaymentType,
            TotalPrice = invoice.TotalPrice,
            Client = invoice.Client.ToDto(),
            InvoiceProducts = invoice.InvoiceProducts,
            UserId = invoice.UserId
        };
    }
    
    public static Invoice ToModel(this CreateInvoiceDto createInvoiceDto)
    {
        return new Invoice()
        {
            Re = createInvoiceDto.Re,
            Tax = createInvoiceDto.Tax,
            TransportPrice = createInvoiceDto.TransportPrice,
            PaymentType = createInvoiceDto.PaymentType,
            ClientId = createInvoiceDto.ClientId,
            InvoiceProducts = createInvoiceDto.InvoiceProducts.Select(x => x.ToModel()).ToList()
        };
    }
    
    public static InvoiceProduct ToModel(this CreateInvoiceProductDto createInvoiceProductDto)
    {
        return new InvoiceProduct()
        {
            ProductId = createInvoiceProductDto.ProductId,
            Quantity = createInvoiceProductDto.Quantity,
            Price = createInvoiceProductDto.Price,
            Discount = createInvoiceProductDto.Discount,
            Reference = createInvoiceProductDto.Reference,
            Description = createInvoiceProductDto.Description
        };
    }
    
    public static ClientDto ToDto(this Client? client)
    {
        if (client is null)
        {
            return null;
        }
        return new ClientDto
        {
            Id = client.Id,
            Name = client.Name,
            ShopName = client.ShopName,
            Address = client.Address,
            City = client.City,
            Province = client.Province,
            Postcode = client.Postcode,
            DocumentNum = client.DocumentNum,
            DocumentType = client.DocumentType,
            Telephone = client.Telephone,
            Email = client.Email,
            UserId = client.UserId
        };
    }
}