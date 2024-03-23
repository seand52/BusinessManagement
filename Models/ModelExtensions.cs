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

    public static decimal CalculateTotalPrice(this IPriceCalculable order)
    {
        var totalPriceOfItems = order.Items.Sum(x => x.Price * x.Quantity * (1 - x.Discount));
        var tax = order.Tax * totalPriceOfItems;
        var re = order.Re * totalPriceOfItems;
        return totalPriceOfItems + tax + re + order.TransportPrice;
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
    
    private static InvoiceProduct ToModel(this CreateInvoiceProductDto createInvoiceProductDto)
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

    public static Client ToModel(this CreateClientDto client)
    {
        return new Client()
        {
            Name = client.Name,
            ShopName = client.ShopName,
            Address = client.Address,
            City = client.City,
            Province = client.Province,
            Postcode = client.Postcode,
            DocumentNum = client.DocumentNum,
            DocumentType = client.DocumentType,
            Telephone = client.Telephone,
            Email = client.Email
        };
    }
    
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto() 
        {
            Id = product.Id,
            Reference = product.Reference,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }
    
    public static Product ToModel(this CreateProductDto product)
    {
        return new Product()
        {
            Reference = product.Reference,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public static BusinessInfo ToModel(this CreateBusinessInfoDto businessInfo)
    {
        return new BusinessInfo()
        {
            Name = businessInfo.Name,
            Cif = businessInfo.Cif,
            Address = businessInfo.Address,
            City = businessInfo.City,
            Postcode = businessInfo.Postcode,
            Country = businessInfo.Country,
            Telephone = businessInfo.Telephone,
            Email = businessInfo.Email
        };
    }
    
    public static BusinessInfoDto ToDto(this BusinessInfo businessInfo)
    {
        return new BusinessInfoDto()
        {
            Id = businessInfo.Id,
            Name = businessInfo.Name,
            Cif = businessInfo.Cif,
            Address = businessInfo.Address,
            City = businessInfo.City,
            Postcode = businessInfo.Postcode,
            Country = businessInfo.Country,
            Telephone = businessInfo.Telephone,
            Email = businessInfo.Email,
            UserId = businessInfo.UserId
        };
    }

    public static SalesOrderDetailDto ToDto(this SalesOrder salesOrder)
    {
        return new SalesOrderDetailDto() 
        {
            Id = salesOrder.Id,
            Re = salesOrder.Re,
            Tax = salesOrder.Tax,
            TransportPrice = salesOrder.TransportPrice,
            PaymentType = salesOrder.PaymentType,
            TotalPrice = salesOrder.TotalPrice,
            Client = salesOrder.Client.ToDto(),
            SalesOrderProducts = salesOrder.SalesOrderProducts,
            UserId = salesOrder.UserId
        };
    }
    
    public static SalesOrder ToModel(this CreateSalesOrderDto salesOrderDto)
    {
        return new SalesOrder()
        {
            Re = salesOrderDto.Re,
            Tax = salesOrderDto.Tax,
            TransportPrice = salesOrderDto.TransportPrice,
            PaymentType = salesOrderDto.PaymentType,
            ClientId = salesOrderDto.ClientId,
            SalesOrderProducts = salesOrderDto.SalesOrderProducts.Select(x => x.ToModel()).ToList()
        };
    }
    
    private static SalesOrderProduct ToModel(this CreateSalesOrderProductDto createSalesOrderProductDto)
    {
        return new SalesOrderProduct()
        {
            ProductId = createSalesOrderProductDto.ProductId,
            Quantity = createSalesOrderProductDto.Quantity,
            Price = createSalesOrderProductDto.Price,
            Discount = createSalesOrderProductDto.Discount,
            Reference = createSalesOrderProductDto.Reference,
            Description = createSalesOrderProductDto.Description
        };
    }
}