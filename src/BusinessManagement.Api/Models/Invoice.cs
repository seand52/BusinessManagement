using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementApi.Models;

public enum PaymentType
{
    Cash,
    Card,
    Transfer
}

public class Invoice: BaseEntity, IPriceCalculable
{
    [Key]
    public int Id { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    [Range(0, 1, ErrorMessage = "The Re must be between 0 and 1.")]
    public decimal Re { get; set; }

    [Required]
    [Range(0, 1, ErrorMessage = "The Tax must be between 0 and 1.")]
    public decimal Tax { get; set; }

    [Required]
    public decimal TransportPrice { get; set; } = 0;

    [Required]
    [EnumDataType(typeof(PaymentType), ErrorMessage = "PaymentType must be one of the following: Cash, Card, Transfer")]
    public PaymentType PaymentType { get; set; }

    // [Required]
    public string UserId { get; set; }
    [Required]
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public List<Product> Products { get; } = [];

    public int InvoiceNumber { get; set; } = 0;
    
    [Required]
    [BindProperty]
    public DateTime DateIssued { get; set; } = DateTime.UtcNow;
    public List<InvoiceProduct> InvoiceProducts { get; set; } = [];
    IEnumerable<ICalculableItem> IPriceCalculable.Items => InvoiceProducts;
}
