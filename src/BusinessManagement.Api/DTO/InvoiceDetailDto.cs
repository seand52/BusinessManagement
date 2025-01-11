using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    public DateTime DateIssued { get; set; } = DateTime.UtcNow;
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
    public string PaymentType { get; set; }
    
    [Required]
    public string UserId { get; set; }
}

public class InvoiceDto
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TransportPrice { get; set; }
    public DateTime DateIssued { get; set; }
    public string PaymentType { get; set; }
    public string ClientName { get; set; }
        
}

public class CreateInvoiceDto : BaseInvoiceDto
{
    [Required]
    public int ClientId { get; set; }
    [Required]
    [EnumDataType(typeof(PaymentType), ErrorMessage = "PaymentType must be one of the following: Cash, Card, Transfer")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentType PaymentType { get; set; }
    [Required]
    public List<CreateInvoiceProductDto> InvoiceProducts { get; set; } = new List<CreateInvoiceProductDto>();
}

public class UpdateInvoiceDto : CreateInvoiceDto
{
}