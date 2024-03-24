using System.ComponentModel.DataAnnotations;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.Dto;

public class BaseInvoiceDto
{
    [Required]
    [Range(0, 1, ErrorMessage = "The Re must be between 0 and 1.")]
    public decimal Re { get; set; }
    [Required]
    [Range(0, 1, ErrorMessage = "The Tax must be between 0 and 1.")]
    public decimal Tax { get; set; }

    [Required]
    public decimal TransportPrice { get; set; } = 0;

    [Required]
    // [EnumDataType(typeof(PaymentType), ErrorMessage = "PaymentType must be one of the following: CASH, CARD, TRANSFER")]
    public PaymentType PaymentType { get; set; }
}

public class InvoiceDetailDto: BaseInvoiceDto
{
    public int Id { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public ClientDto Client { get; set; }
    [Required]
    public List<InvoiceProduct> InvoiceProducts { get; set; }
    [Required]
    public string UserId { get; set; }
}

public class InvoiceDto
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public ClientDto Client { get; set; }
    public decimal TransportPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public PaymentType PaymentType { get; set; }
        
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