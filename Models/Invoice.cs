using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public enum PaymentType
{
    CASH,
    CARD,
    TRANSFER
}

public class Invoice
{
    public int Id { get; set; }

    [Required]
    public double TotalPrice { get; set; }

    [Required]
    public double Re { get; set; }

    [Required]
    public double Tax { get; set; }

    [Required]
    public double TransportPrice { get; set; } = 0;

    [Required]
    // [EnumDataType(typeof(PaymentType), ErrorMessage = "PaymentType must be one of the following: CASH, CARD, TRANSFER")]
    public PaymentType PaymentType { get; set; }

    // [Required]
    public string UserId { get; set; }
    [Required]
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public List<Product> Products { get; } = [];
    public List<InvoiceProduct> InvoiceProducts { get; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
