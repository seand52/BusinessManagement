using System.ComponentModel.DataAnnotations;
using BusinessManagementApi.Models;

namespace BusinessManagementApi.Dto;

public class BaseSalesOrderDto
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
    public DateTime DateIssued { get; set; }

    [Required]
    // [EnumDataType(typeof(PaymentType), ErrorMessage = "PaymentType must be one of the following: CASH, CARD, TRANSFER")]
    public PaymentType PaymentType { get; set; }
}

public class SalesOrderDetailDto: BaseSalesOrderDto
{
    public int Id { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public ClientDto Client { get; set; }

    [Required] public List<SalesOrderProduct> SalesOrderProducts { get; set; } = [];
    [Required]
    public string UserId { get; set; }
}

public class SalesOrderDto
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public ClientDto Client { get; set; }
    public decimal TransportPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public PaymentType PaymentType { get; set; }
        
}

public class CreateSalesOrderDto : BaseSalesOrderDto
{
    [Required]
    public int ClientId { get; set; }
    public byte Expired { get; set; } = 0;

    [Required] public List<CreateSalesOrderProductDto> SalesOrderProducts { get; set; } = [];
}

public class UpdateSalesOrderDto : CreateSalesOrderDto
{
    
}
