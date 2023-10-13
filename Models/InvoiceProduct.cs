using System.ComponentModel.DataAnnotations;

namespace BusinessManagementApi.Models;

public class InvoiceProduct {
    public int Id { get; set; }
    [Required]
    public int InvoiceId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Discount { get; set; }
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public string Reference { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}