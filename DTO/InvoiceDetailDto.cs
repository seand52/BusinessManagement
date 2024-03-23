using System.ComponentModel.DataAnnotations;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.Dto;

public class BaseInvoiceDto
{
    [Required]
    public double Re { get; set; }
    [Required]
    public double Tax { get; set; }

    [Required]
    public double TransportPrice { get; set; } = 0;

    [Required]
    // [EnumDataType(typeof(PaymentType), ErrorMessage = "PaymentType must be one of the following: CASH, CARD, TRANSFER")]
    public PaymentType PaymentType { get; set; }
}

public class InvoiceDetailDto: BaseInvoiceDto
{
    public int Id { get; set; }
    [Required]
    public double TotalPrice { get; set; }
    [Required]
    public ClientDto Client { get; set; }
    [Required]
    public List<InvoiceProduct> InvoiceProducts { get; set; }
    [Required]
    public string UserId { get; set; }
}

public class CreateInvoiceDto : BaseInvoiceDto
{
    [Required]
    public int ClientId { get; set; }
    [Required]
    public List<CreateInvoiceProductDto> InvoiceProducts { get; set; } = new List<CreateInvoiceProductDto>();
}

public class UpdateInvoiceDto : CreateInvoiceDto
{
}